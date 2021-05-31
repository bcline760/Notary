using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Linq;

using log4net;

using Org.BouncyCastle.X509;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Nist;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Operators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Encoders;

using Notary.Configuration;
using Notary.Contract;
using Notary.Interface.Repository;
using Notary.Interface.Service;
using Notary.Logging;

namespace Notary.Service
{
    public class CertificateService : EntityService<Certificate>, ICertificateService
    {
        public CertificateService(
            NotaryConfiguration config,
            NotaryCaConfiguration caConfig,
            ICertificateRepository repository,
            IRevocatedCertificateRepository revocatedCertificateRepo,
            ILog log) : base(repository, log)
        {
            Configuration = config;
            CaConfiguration = caConfig;
            RevocatedCertificateRepository = revocatedCertificateRepo;
        }

        public async Task IssueCertificateAsync(CertificateRequest request)
        {
            try
            {
                var signingCert = await GetSigningCertificateAsync();
                if (signingCert == null)
                    throw new InvalidOperationException("Signing certificate doesn't exist.");

                //Load the issuer's private key from the file system.
                
                string signingPrivateKeyPath = $"{CaConfiguration.SigningCertificatePath}/{Constants.KeyDirectoryPath}/{signingCert.Thumbprint}.key.pem";
                var issuerKeyPair = LoadKeyPair(signingPrivateKeyPath, Configuration.ApplicationKey, CaConfiguration.CaKeyAlgorithm);

                var issuerSn = new BigInteger(signingCert.SerialNumber, 16);
                var random = GetSecureRandom();
                var certificateKeyPair = GenerateKeyPair(random, CaConfiguration.KeySize, CaConfiguration.CaKeyAlgorithm);
                var serialNumber = GenerateSerialNumber(random);
                var certificateDn = DistinguishedName.BuildDistinguishedName(request.Subject);
                var notAfter = DateTime.UtcNow.AddHours(request.LengthInHours);
                var issuerDn = DistinguishedName.BuildDistinguishedName(signingCert.Subject);
                var keyUsages = new KeyPurposeID[]
                {
                    KeyPurposeID.IdKPServerAuth,
                    KeyPurposeID.IdKPClientAuth
                };

                //Generate the certificate
                var generatedCertificate = GenerateCertificate(
                    request.SubjectAlternativeNames,
                    random,
                    certificateDn,
                    certificateKeyPair,
                    serialNumber,
                    issuerDn,
                    notAfter,
                    issuerKeyPair,
                    issuerSn,
                    false,
                    keyUsages
                );

                var thumbprint = GetThumbprint(generatedCertificate);

                var issuedKeyPath = $"{CaConfiguration.IssuedCertificatePath}/{Constants.KeyDirectoryPath}/{thumbprint}.key.pem";
                var issuedCertPath = $"{CaConfiguration.IssuedCertificatePath}/{Constants.CertificateDirectoryPath}/{thumbprint}.cer";

                //Save encrypted private key and certificate to the file system
                SavePrivateKey(certificateKeyPair, issuedKeyPath, random, request.CertificatePassword);
                SaveCertificate(generatedCertificate, issuedCertPath);

                var certificate = new Certificate
                {
                    Active = true,
                    Algorithm = Algorithm.RSA,
                    Created = DateTime.UtcNow,
                    CreatedBySlug = request.RequestedBySlug,
                    Issuer = signingCert.Issuer,
                    KeyUsage = 0,
                    NotAfter = notAfter,
                    NotBefore = DateTime.UtcNow,
                    PrimarySigningCertificate = false,
                    SerialNumber = generatedCertificate.SerialNumber.ToString(16),
                    Subject = request.Subject,
                    SigningCertificateSlug = signingCert.Slug,
                    SubjectAlternativeNames = request.SubjectAlternativeNames,
                    Thumbprint = thumbprint
                };
                await SaveAsync(certificate, request.RequestedBySlug);
            }
            catch (Exception ex)
            {
                throw ex.IfNotLoggedThenLog(Logger);
            }
        }

        public async Task<byte[]> DownloadCertificateAsync(string slug, CertificateFormat format, string privateKeyPassword)
        {
            var certificate = await GetAsync(slug);
            if (certificate == null)
                return null;

            byte[] certificateBinary = null;

            var certPath = $"{CaConfiguration.IssuedCertificatePath}/{Constants.CertificateDirectoryPath}/{certificate.Thumbprint}.cer";
            X509Certificate cert = await LoadCertificate(certPath);
            if (cert == null)
                return null;
            switch (format)
            {
                case CertificateFormat.Der:
                    certificateBinary = cert.GetEncoded();
                    break;
                case CertificateFormat.Pem:
                    break;
                case CertificateFormat.Pkcs12:
                    var certKeyPath = $"{CaConfiguration.IssuedCertificatePath}/{Constants.KeyDirectoryPath}/{certificate.Thumbprint}.key.pem";
                    var certKey = LoadKeyPair(certKeyPath, privateKeyPassword, CaConfiguration.CaKeyAlgorithm);
                    var store = new Pkcs12StoreBuilder().Build();
                    var certEntry = new X509CertificateEntry(cert);
                    var keyEntry = new AsymmetricKeyEntry(certKey.Private);

                    store.SetKeyEntry(certificate.Subject.ToString(), keyEntry, new X509CertificateEntry[] { certEntry });
                    using (var memStream = new MemoryStream())
                    {
                        store.Save(memStream, privateKeyPassword.ToArray(), GetSecureRandom());
                        certificateBinary = memStream.ToArray();
                    }
                    break;
                case CertificateFormat.Pkcs7:
                    break;
                default:
                    throw new ArgumentException(nameof(format));
            }

            return certificateBinary;
        }

        public async Task<List<RevocatedCertificate>> GetRevocatedCertificates()
        {
            var revocatedCerts = await RevocatedCertificateRepository.GetAllAsync();
            return revocatedCerts;
        }

        public async Task RevokeCertificateAsync(string slug, RevocationReason reason, string userRevocatingSlug)
        {
            var certificate = await GetAsync(slug);

            if (certificate != null)
            {
                certificate.RevocationDate = DateTime.UtcNow;
                await SaveAsync(certificate, userRevocatingSlug);

                var revocatedCertificate = new RevocatedCertificate
                {
                    Active = true,
                    Created = DateTime.Now,
                    CreatedBySlug = userRevocatingSlug,
                    Reason = reason,
                    SerialNumber = certificate.SerialNumber,
                    Thumbprint = certificate.Thumbprint
                };

                await RevocatedCertificateRepository.SaveAsync(revocatedCertificate);
            }
        }

        public async Task GenerateCaCertificates(CertificateAuthoritySetup setup)
        {
            var randomRoot = GetSecureRandom();
            var snRoot = GenerateSerialNumber(randomRoot);
            var rootKeyPair = GenerateKeyPair(randomRoot, setup.KeyLength, CaConfiguration.CaKeyAlgorithm);

            var signingRandom = GetSecureRandom();
            var snIntermediate = GenerateSerialNumber(signingRandom);
            var signingKeyPair = GenerateKeyPair(signingRandom, setup.KeyLength, CaConfiguration.CaKeyAlgorithm);

            X509Certificate rootCertificate = null, signingCertificate = null;
            try
            {
                rootCertificate = GenerateCertificate(
                    new List<SubjectAlternativeName>(),
                    randomRoot,
                    DistinguishedName.BuildDistinguishedName(setup.RootDn),
                    rootKeyPair,
                    snRoot,
                    DistinguishedName.BuildDistinguishedName(setup.RootDn), //Root certs self signed
                    DateTime.UtcNow.AddYears(setup.LengthInYears),
                    rootKeyPair, //Root certs, self signed
                    snRoot,
                    true,
                    new KeyPurposeID[]
                    {
                    KeyPurposeID.IdKPCodeSigning
                    });

                var rootThumb = GetThumbprint(rootCertificate);

                var rootCertificateData = new Certificate
                {
                    Active = true,
                    Algorithm = Algorithm.RSA,
                    Created = DateTime.Now,
                    Issuer = setup.RootDn,
                    SubjectAlternativeNames = new List<SubjectAlternativeName>(),
                    Subject = setup.RootDn,
                    KeyUsage = (int)KeyUsageFlags.CodeSigning,
                    NotAfter = DateTime.UtcNow.AddYears(setup.LengthInYears),
                    NotBefore = DateTime.UtcNow,
                    CreatedBySlug = setup.Requestor,
                    PrimarySigningCertificate = false,
                    SerialNumber = rootCertificate.SerialNumber.ToString(16),
                    Thumbprint = rootThumb
                };

                //Persist the certificate to the data store.
                await SaveAsync(rootCertificateData, setup.Requestor);

                signingCertificate = GenerateCertificate(
                    new List<SubjectAlternativeName>(),
                    signingRandom,
                    DistinguishedName.BuildDistinguishedName(setup.SigningDn),
                    signingKeyPair,
                    snIntermediate,
                    DistinguishedName.BuildDistinguishedName(setup.RootDn),
                    DateTime.UtcNow.AddYears(setup.LengthInYears),
                    rootKeyPair,
                    snRoot,
                    true,
                    new KeyPurposeID[]
                    {
                    KeyPurposeID.IdKPClientAuth,
                    KeyPurposeID.IdKPCodeSigning,
                    KeyPurposeID.IdKPEmailProtection,
                    KeyPurposeID.IdKPOcspSigning,
                    KeyPurposeID.IdKPServerAuth,
                    KeyPurposeID.IdKPTimeStamping
                    });

                var interThumb = GetThumbprint(signingCertificate);

                var signingCertificateData = new Certificate
                {
                    Active = true,
                    Algorithm = Algorithm.RSA,
                    Created = DateTime.Now,
                    Issuer = setup.RootDn,
                    SubjectAlternativeNames = new List<SubjectAlternativeName>(),
                    Subject = setup.SigningDn,
                    KeyUsage = (int)KeyUsageFlags.CodeSigning,
                    NotAfter = DateTime.UtcNow.AddYears(setup.LengthInYears),
                    NotBefore = DateTime.UtcNow,
                    SigningCertificateSlug = rootCertificateData.Slug,
                    CreatedBySlug = setup.Requestor,
                    PrimarySigningCertificate = true,
                    SerialNumber = signingCertificate.SerialNumber.ToString(),
                    Thumbprint = interThumb
                };

                await SaveAsync(signingCertificateData, setup.Requestor);

                string rootPkPath = $"{CaConfiguration.RootCertificatePath}/{Constants.KeyDirectoryPath}/{rootCertificateData.Thumbprint}.key.pem";
                string signingPkPath = $"{CaConfiguration.RootCertificatePath}/{Constants.KeyDirectoryPath}/{signingCertificateData.Thumbprint}.key.pem";
                SavePrivateKey(rootKeyPair, rootPkPath, randomRoot, Configuration.ApplicationKey);
                SavePrivateKey(signingKeyPair, signingPkPath, signingRandom, Configuration.ApplicationKey);

                string rootCertPath = $"{CaConfiguration.SigningCertificatePath}/{Constants.CertificateDirectoryPath}/{rootCertificateData.Thumbprint}.cer";
                string signingCertificatePath = $"{CaConfiguration.SigningCertificatePath}/{Constants.CertificateDirectoryPath}/{signingCertificateData.Thumbprint}.cer";
                SaveCertificate(rootCertificate, rootCertPath);
                SaveCertificate(signingCertificate, signingCertificatePath);
            }
            catch (Exception cex)
            {
                throw cex.IfNotLoggedThenLog(Logger);
            }
        }

        public async Task<Certificate> GetSigningCertificateAsync()
        {
            var repo = (ICertificateRepository)Repository;

            return await repo.GetSigningCertificateAsync();
        }

        #region Private Methods

        /// <summary>
        /// Generate a cryptographically secure random number
        /// </summary>
        /// <returns>The cryptographically secure random number generated object</returns>
        private static SecureRandom GetSecureRandom()
        {
            var random = new SecureRandom();
            return random;
        }


        /// <summary>
        /// Generate a key pair.
        /// </summary>
        /// <param name="random">The random number generator.</param>
        /// <param name="strength">The key length in bits. For RSA, 2048 bits should be considered the minimum acceptable these days.</param>
        /// <returns></returns>
        private static AsymmetricCipherKeyPair GenerateKeyPair(SecureRandom random, int strength, Algorithm algorithm)
        {
            AsymmetricCipherKeyPair keyPair = null;

            if (algorithm == Algorithm.RSA)
            {
                var keyGenerationParameters = new KeyGenerationParameters(random, strength);
                var keyPairGenerator = new RsaKeyPairGenerator();
                keyPairGenerator.Init(keyGenerationParameters);
                keyPair = keyPairGenerator.GenerateKeyPair();
            }
            else if (algorithm == Algorithm.EllipticCurve)
            {
                var ecp = NistNamedCurves.GetByName("P-521"); //TODO: Refactor

                var curve = (FpCurve)ecp.Curve;
                var domain = new ECDomainParameters(curve, ecp.G, ecp.N, ecp.H);

                var keyGenerationParameters = new ECKeyGenerationParameters(domain, random);
                var ecGenerator = new ECKeyPairGenerator();
                ecGenerator.Init(keyGenerationParameters);

                keyPair = ecGenerator.GenerateKeyPair();
            }

            return keyPair;
        }


        /// <summary>
        /// Create a serial number used for certificate and other cryptography objects
        /// </summary>
        /// <param name="random">A crypto-random number generated</param>
        /// <returns>An integer value representing the serial number</returns>
        private static BigInteger GenerateSerialNumber(SecureRandom random)
        {
            var serialNumber =
                BigIntegers.CreateRandomInRange(
                    BigInteger.One, BigInteger.ValueOf(Int64.MaxValue), random);
            return serialNumber;
        }

        private static AsymmetricCipherKeyPair LoadKeyPair(string filePath, string encryptionKey, Algorithm algorithm)
        {
            AsymmetricCipherKeyPair keyPair = null;
            using (FileStream fs = File.OpenRead(filePath))
            using (TextReader tr = new StreamReader(fs))
            {
                PemReader pr = new PemReader(tr, new PasswordFinder(encryptionKey));
                
                if (algorithm == Algorithm.RSA)
                {
                    var pemObject = pr.ReadObject();
                    var privateKey = (RsaPrivateCrtKeyParameters)pemObject;
                    var publicKey = new RsaKeyParameters(false, privateKey.Modulus, privateKey.PublicExponent);
                    keyPair = new AsymmetricCipherKeyPair(publicKey, privateKey);
                }
                else if (algorithm == Algorithm.EllipticCurve)
                {
                    var pem = pr.ReadPemObject();
                    var ellipticCurve = NistNamedCurves.GetByName("P-521"); //TODO: Refactor

                    var curve = (FpCurve)ellipticCurve.Curve;
                    var domain = new ECDomainParameters(curve, ellipticCurve.G, ellipticCurve.N, ellipticCurve.H);

                    var d = new BigInteger(pem.Content);
                    var q = domain.G.Multiply(d);

                    var publicKey = new ECPublicKeyParameters(q, domain);
                }
            }

            return keyPair;
        }

        private static void SavePrivateKey(AsymmetricCipherKeyPair keyPair, string filePath, SecureRandom encryptionRandom, string pkPassword)
        {
            var generator = new Pkcs8Generator(keyPair.Private, Pkcs8Generator.PbeSha1_3DES);
            generator.Password = pkPassword.ToCharArray();
            generator.SecureRandom = encryptionRandom;
            generator.IterationCount = 32;

            var pemObject = generator.Generate();
            using (FileStream fs = File.OpenWrite(filePath))
            using (TextWriter tw = new StreamWriter(fs))
            {
                PemWriter pemWriter = new PemWriter(tw);
                pemWriter.WriteObject(pemObject);
                pemWriter.Writer.Flush();
            }
        }

        #region Certificate Private Methods

        /// <summary>
        /// Add the Authority Key Identifier. According to http://www.alvestrand.no/objectid/2.5.29.35.html, this
        /// identifies the public key to be used to verify the signature on this certificate.
        /// In a certificate chain, this corresponds to the "Subject Key Identifier" on the *issuer* certificate.
        /// The Bouncy Castle documentation, at http://www.bouncycastle.org/wiki/display/JA1/X.509+Public+Key+Certificate+and+Certification+Request+Generation,
        /// shows how to create this from the issuing certificate. Since we're creating a self-signed certificate, we have to do this slightly differently.
        /// </summary>
        /// <param name="certificateGenerator">The object used to generate certificate</param>
        /// <param name="issuerDN">The issuer's distinguished name</param>
        /// <param name="issuerKeyPair">The issuer's key pair</param>
        /// <param name="issuerSerialNumber">The issuer's serial number</param>
        private static void AddAuthorityKeyIdentifier(X509V3CertificateGenerator certificateGenerator,
                                                      X509Name issuerDN,
                                                      AsymmetricCipherKeyPair issuerKeyPair,
                                                      BigInteger issuerSerialNumber)
        {
            var authorityKeyIdentifierExtension =
                new AuthorityKeyIdentifier(
                    SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(issuerKeyPair.Public),
                    new GeneralNames(new GeneralName(issuerDN)),
                    issuerSerialNumber);
            certificateGenerator.AddExtension(
                X509Extensions.AuthorityKeyIdentifier.Id, false, authorityKeyIdentifierExtension);
        }

        /// <summary>
        /// Add the "Extended Key Usage" extension, specifying (for example) "server authentication".
        /// </summary>
        /// <param name="certificateGenerator"></param>
        /// <param name="usages"></param>
        private static void AddExtendedKeyUsage(X509V3CertificateGenerator certificateGenerator, KeyPurposeID[] usages)
        {
            certificateGenerator.AddExtension(
                X509Extensions.ExtendedKeyUsage.Id, false, new ExtendedKeyUsage(usages));
        }

        /// <summary>
        /// Add the "Basic Constraints" extension.
        /// </summary>
        /// <param name="certificateGenerator"></param>
        /// <param name="isCertificateAuthority"></param>
        private static void AddBasicConstraints(X509V3CertificateGenerator certificateGenerator,
                                                bool isCertificateAuthority)
        {
            certificateGenerator.AddExtension(
                X509Extensions.BasicConstraints.Id, true, new BasicConstraints(isCertificateAuthority));
        }

        /// <summary>
        /// Add the Subject Key Identifier.
        /// </summary>
        /// <param name="certificateGenerator"></param>
        /// <param name="subjectKeyPair"></param>
        private static void AddSubjectKeyIdentifier(X509V3CertificateGenerator certificateGenerator,
                                                    AsymmetricCipherKeyPair subjectKeyPair)
        {
            var subjectKeyIdentifierExtension =
                new SubjectKeyIdentifier(
                    SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(subjectKeyPair.Public));
            certificateGenerator.AddExtension(
                X509Extensions.SubjectKeyIdentifier.Id, false, subjectKeyIdentifierExtension);
        }

        /// <summary>
        /// Add the "Subject Alternative Names" extension. Note that you have to repeat
        /// the value from the "Subject Name" property.
        /// </summary>
        /// <param name="certificateGenerator"></param>
        /// <param name="subjectAlternativeNames"></param>
        private static void AddSubjectAlternativeNames(X509V3CertificateGenerator certificateGenerator,
                                                       IEnumerable<SubjectAlternativeName> subjectAlternativeNames)
        {
            var sanExtension = new DerSequence(subjectAlternativeNames.Select(n =>
            {
                int generalName = -1;
                switch (n.Kind)
                {
                    case SanKind.Dns:
                        generalName = GeneralName.DnsName;
                        break;
                    case SanKind.Email:
                        generalName = GeneralName.X400Address;
                        break;
                    case SanKind.IpAddress:
                        generalName = GeneralName.IPAddress;
                        break;
                    case SanKind.UserPrincipal:
                        generalName = GeneralName.DirectoryName;
                        break;
                    case SanKind.Uri:
                        generalName = GeneralName.UniformResourceIdentifier;
                        break;
                    default:
                        break;
                }

                return new GeneralName(generalName, n.Name);
            }).ToArray<Asn1Encodable>());

            certificateGenerator.AddExtension(X509Extensions.SubjectAlternativeName, false, sanExtension);
        }

        private static X509Certificate GenerateCertificate(List<SubjectAlternativeName> sanList, SecureRandom random,
            string subjectDn, AsymmetricCipherKeyPair subjectKeyPair, BigInteger subjectSn, string issuerDn, DateTime notAfter,
            AsymmetricCipherKeyPair issuerKeyPair, BigInteger issuerSn, bool isCA, KeyPurposeID[] usages)
        {
            var certGen = new X509V3CertificateGenerator();
            var subject = new X509Name(subjectDn);
            var issuer = new X509Name(issuerDn);
            var notBefore = DateTime.UtcNow;
            var signatureFactory = new Asn1SignatureFactory("SHA256WithRSA", issuerKeyPair.Private);

            certGen.SetPublicKey(subjectKeyPair.Public);
            certGen.SetSerialNumber(subjectSn);
            certGen.SetSubjectDN(subject);
            certGen.SetIssuerDN(issuer);
            certGen.SetNotBefore(notBefore);
            certGen.SetNotAfter(notAfter);

            AddAuthorityKeyIdentifier(certGen, issuer, issuerKeyPair, issuerSn);
            AddSubjectKeyIdentifier(certGen, subjectKeyPair);
            AddBasicConstraints(certGen, isCA);

            if (usages != null && usages.Any())
                AddExtendedKeyUsage(certGen, usages);

            if (sanList != null && sanList.Any())
                AddSubjectAlternativeNames(certGen, sanList);

            var bouncyCert = certGen.Generate(signatureFactory);

            return bouncyCert;
        }

        /// <summary>
        /// Load a certificate from disk
        /// </summary>
        /// <param name="certPath"></param>
        /// <returns>An X.509 certificate or null if it is not on disk</returns>
        private static async Task<X509Certificate> LoadCertificate(string certPath)
        {
            X509Certificate cert = null;

            using (FileStream fs = File.OpenRead(certPath))
            {
                var mem = new Memory<byte>();
                int bytesRead = await fs.ReadAsync(mem);

                //Ensure every byte read
                if (mem.Length == bytesRead)
                {
                    var parser = new X509CertificateParser();
                    cert = parser.ReadCertificate(mem.ToArray());
                }
            }

            return null;
        }

        /// <summary>
        /// Save the certificate binary to disk
        /// </summary>
        /// <param name="certificate">The X.509 certificate</param>
        /// <param name="filePath">Path on disk of the </param>
        private static void SaveCertificate(X509Certificate certificate, string filePath)
        {
            using (FileStream fs = File.OpenWrite(filePath))
            {
                fs.Write(certificate.GetEncoded());
            }
        }

        /// <summary>
        /// Get the certificate thumbprint/fingerprint
        /// </summary>
        /// <param name="certificate">The certificate for finding thumbprint</param>
        /// <returns>The SHA256 thumbprint of the certificate</returns>
        private static string GetThumbprint(X509Certificate certificate)
        {
            byte[] certData = certificate.GetEncoded();

            var digest = new Sha256Digest();
            digest.BlockUpdate(certData, 0, certData.Length);
            byte[] digestedCert = new byte[digest.GetDigestSize()];
            digest.DoFinal(digestedCert, 0);
            byte[] hexBytes = Hex.Encode(digestedCert);

            return Encoding.ASCII.GetString(hexBytes);
        }

        #endregion

        #endregion

        protected NotaryConfiguration Configuration { get; }

        protected NotaryCaConfiguration CaConfiguration { get; }

        protected IRevocatedCertificateRepository RevocatedCertificateRepository { get; }
    }
}
