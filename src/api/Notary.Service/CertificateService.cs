using System;
using System.Collections.Generic;
using X509 = System.Security.Cryptography.X509Certificates;
using System.IO;
using System.Threading.Tasks;
using System.Linq;

using log4net;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Operators;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Crypto.Parameters;

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
            ICertificateRepository repository,
            ILog log,
            IEncryptionService encryption) : base(repository, log)
        {
            EncryptionService = encryption;
            Configuration = config;
        }

        public async Task IssueCertificateAsync(CertificateRequest request)
        {
            try
            {
                var signingCert = await GetSigningCertificateAsync();
                if (signingCert == null)
                    throw new InvalidOperationException("Signing certificate doesn't exist.");

                //Load the issuer's private key from the file system.
                string signingPrivateKeyPath = $"{Configuration.Intermediate.PrivateKeyDirectory}/{signingCert.Thumbprint}.key.pem";
                var issuerKeyPair = EncryptionService.LoadKeyPair(signingPrivateKeyPath, Configuration.EncryptionKey);

                var issuerSn = new BigInteger(signingCert.SerialNumber, 16);
                var random = EncryptionService.GetSecureRandom();
                var certificateKeyPair = EncryptionService.GenerateKeyPair(random, 2048);
                var serialNumber = EncryptionService.GenerateSerialNumber(random);
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

                var issuedKeyPath = $"{Configuration.Issued.PrivateKeyDirectory}/{generatedCertificate.Thumbprint}.key.pem";
                var issuedCertPath = $"{Configuration.Issued.CertificateDirectory}/{generatedCertificate.Thumbprint}.cer";

                //Save encrypted private key and certificate to the file system
                EncryptionService.SavePrivateKey(certificateKeyPair, issuedKeyPath, random);
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
                    SerialNumber = generatedCertificate.SerialNumber,
                    Subject = request.Subject,
                    SigningCertificateSlug = signingCert.Slug,
                    SubjectAlternativeNames = request.SubjectAlternativeNames,
                    Thumbprint = generatedCertificate.Thumbprint
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
            byte[] certificateBinary = null;

            if (certificate != null)
            {
                
            }

            return certificateBinary;
        }

        public async Task<List<RevocatedCertificate>> GetRevocatedCertificates()
        {
            throw new NotImplementedException();
        }

        public async Task RevokeCertificateAsync(string slug, RevocationReason reason)
        {
            var certificate = await GetAsync(slug);

            if (certificate != null)
            {
                certificate.RevocationDate = DateTime.UtcNow;
            }
        }

        public async Task GenerateCaCertificates(CertificateRequest root, CertificateRequest intermediate)
        {
            var randomRoot = EncryptionService.GetSecureRandom();
            var snRoot = EncryptionService.GenerateSerialNumber(randomRoot);
            var rootKeyPair = EncryptionService.GenerateKeyPair(randomRoot, 2048);

            var intermediateRandom = EncryptionService.GetSecureRandom();
            var snIntermediate = EncryptionService.GenerateSerialNumber(intermediateRandom);
            var intermediateKeyPair = EncryptionService.GenerateKeyPair(intermediateRandom, 2048);

            X509.X509Certificate2 rootCertificate = null, intermediateCertificate = null;
            try
            {
                rootCertificate = GenerateCertificate(
                    root.SubjectAlternativeNames,
                    randomRoot,
                    DistinguishedName.BuildDistinguishedName(root.Subject),
                    rootKeyPair,
                    snRoot,
                    DistinguishedName.BuildDistinguishedName(root.Subject), //Root certs self signed
                    DateTime.UtcNow.AddHours(root.LengthInHours),
                    rootKeyPair, //Root certs, self signed
                    snRoot,
                    true,
                    new KeyPurposeID[]
                    {
                    KeyPurposeID.IdKPCodeSigning
                    });

                var rootCertificateData = new Certificate
                {
                    Active = true,
                    Algorithm = Algorithm.RSA,
                    Created = DateTime.Now,
                    Issuer = root.Subject,
                    SubjectAlternativeNames = root.SubjectAlternativeNames,
                    Subject = root.Subject,
                    KeyUsage = (int)KeyUsageFlags.CodeSigning,
                    NotAfter = DateTime.UtcNow.AddHours(root.LengthInHours),
                    NotBefore = DateTime.UtcNow,
                    CreatedBySlug = root.RequestedBySlug,
                    PrimarySigningCertificate = false,
                    SerialNumber = rootCertificate.SerialNumber,
                    Thumbprint = rootCertificate.Thumbprint
                };

                //Persist the certificate to the data store.
                await SaveAsync(rootCertificateData, "root");

                intermediateCertificate = GenerateCertificate(
                    intermediate.SubjectAlternativeNames,
                    intermediateRandom,
                    DistinguishedName.BuildDistinguishedName(intermediate.Subject),
                    intermediateKeyPair,
                    snIntermediate,
                    DistinguishedName.BuildDistinguishedName(root.Subject),
                    DateTime.UtcNow.AddHours(intermediate.LengthInHours),
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

                var intermediateCertificateData = new Certificate
                {
                    Active = true,
                    Algorithm = Algorithm.RSA,
                    Created = DateTime.Now,
                    Issuer = root.Subject,
                    SubjectAlternativeNames = intermediate.SubjectAlternativeNames,
                    Subject = intermediate.Subject,
                    KeyUsage = (int)KeyUsageFlags.CodeSigning,
                    NotAfter = DateTime.UtcNow.AddHours(intermediate.LengthInHours),
                    NotBefore = DateTime.UtcNow,
                    SigningCertificateSlug = rootCertificateData.Slug,
                    CreatedBySlug = intermediate.RequestedBySlug,
                    PrimarySigningCertificate = true,
                    SerialNumber = intermediateCertificate.SerialNumber,
                    Thumbprint = intermediateCertificate.Thumbprint
                };

                await SaveAsync(intermediateCertificateData, intermediate.RequestedBySlug);

                string rootPkPath = $"{Configuration.Root.PrivateKeyDirectory}/{rootCertificateData.Thumbprint}.key.pem";
                string intermediatePkPath = $"{Configuration.Intermediate.PrivateKeyDirectory}/{intermediateCertificateData.Thumbprint}.key.pem";
                EncryptionService.SavePrivateKey(rootKeyPair, rootPkPath, randomRoot);
                EncryptionService.SavePrivateKey(intermediateKeyPair, intermediatePkPath, intermediateRandom);

                string rootCertPath = $"{Configuration.Root.CertificateDirectory}/{rootCertificateData.Thumbprint}.cer";
                string intermediateCertPath = $"{Configuration.Intermediate.CertificateDirectory}/{intermediateCertificateData.Thumbprint}.cer";
                SaveCertificate(rootCertificate, rootCertPath);
                SaveCertificate(intermediateCertificate, intermediateCertPath);
            }
            catch (CryptoException cex)
            {
                throw cex.IfNotLoggedThenLog(Logger);
            }
            finally
            {
                if (rootCertificate != null)
                    rootCertificate.Dispose();
                if (intermediateCertificate != null)
                    intermediateCertificate.Dispose();
            }
        }

        public async Task<Certificate> GetSigningCertificateAsync()
        {
            var repo = (ICertificateRepository)Repository;

            return await repo.GetSigningCertificateAsync();
        }

        #region Private Methods
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

        private static X509.X509Certificate2 GenerateCertificate(List<SubjectAlternativeName> sanList, SecureRandom random,
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
            X509.X509Certificate2 certificate;

            Pkcs12Store store = new Pkcs12StoreBuilder().Build();
            using (var ms = new MemoryStream())
            {
                var exportPw = Guid.NewGuid().ToString("x");
                store.Save(ms, exportPw.ToArray(), random);
                certificate = new X509.X509Certificate2(ms.ToArray(), exportPw, X509.X509KeyStorageFlags.Exportable);
                return certificate;
            }
        }

        private static void SaveCertificate(X509.X509Certificate2 certificate, string filePath)
        {
            byte[] certificateBinary = certificate.RawData;

            using (FileStream fs = File.OpenWrite(filePath))
            {
                fs.Write(certificateBinary);
            }
        }
        #endregion

        public IEncryptionService EncryptionService { get; }

        public NotaryConfiguration Configuration { get; }
    }
}
