

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.IO;

using Microsoft.IdentityModel.Tokens;

using Notary.Interface.Service;

using log4net;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Utilities;

using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;

namespace Notary.Service
{
    public class EncryptionService : IEncryptionService
    {
        private NotaryConfiguration _config;
        private ILog _log;

        public EncryptionService(NotaryConfiguration config, ILog log)
        {
            _config = config;
            _log = log;
        }

        public byte[] Decrypt(byte[] encryptedData, string accountSlug)
        {
            string path = $"{_config.UserKeyPath}/{accountSlug}.key.pem";

            var keyPair = LoadKeyPair(path, _config.EncryptionKey);
            var decryptEngine = new Pkcs1Encoding(new RsaEngine());
            decryptEngine.Init(false, keyPair.Private);

            var decryptedBytes = decryptEngine.ProcessBlock(encryptedData, 0, encryptedData.Length);
            return decryptedBytes;
        }

        public byte[] Encrypt(byte[] data, string accountSlug)
        {
            string path = $"{_config.UserKeyPath}/{accountSlug}.key.pem";

            var keyPair = LoadKeyPair(path, _config.EncryptionKey);
            var encryptEngine = new Pkcs1Encoding(new RsaEngine());
            encryptEngine.Init(true, keyPair.Public);

            var encryptedBytes = encryptEngine.ProcessBlock(data, 0, data.Length);
            return encryptedBytes;
        }

        public string Hash(string content)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            byte[] buffer = Encoding.Default.GetBytes(content);
            byte[] hashedBuffer = null;
            using (SHA256 s256 = SHA256.Create())
            {
                hashedBuffer = s256.ComputeHash(buffer);
            }

            var sb = new StringBuilder();
            for (int i = 0; i < hashedBuffer.Length; i++)
            {
                sb.Append($"{hashedBuffer[i]:X2}");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Generate an RSA key pair
        /// </summary>
        /// <param name="strength"></param>
        /// <returns>The RSA public and private key pairing</returns>
        public AsymmetricCipherKeyPair GenerateKeyPair(int strength)
        {
            var secureRandom = GetSecureRandom();
            return GenerateKeyPair(secureRandom, strength);
        }

        /// <summary>
        /// Generate a key pair.
        /// </summary>
        /// <param name="random">The random number generator.</param>
        /// <param name="strength">The key length in bits. For RSA, 2048 bits should be considered the minimum acceptable these days.</param>
        /// <returns></returns>
        public AsymmetricCipherKeyPair GenerateKeyPair(SecureRandom random, int strength)
        {
            var keyGenerationParameters = new KeyGenerationParameters(random, strength);

            var keyPairGenerator = new RsaKeyPairGenerator();
            keyPairGenerator.Init(keyGenerationParameters);
            var subjectKeyPair = keyPairGenerator.GenerateKeyPair();
            return subjectKeyPair;
        }

        /// <summary>
        /// Generate a cryptographically secure random number
        /// </summary>
        /// <returns>The cryptographically secure random number generated object</returns>
        public SecureRandom GetSecureRandom()
        {
            var random = new SecureRandom();
            return random;
        }

        /// <summary>
        /// Create a serial number used for certificate and other cryptography objects
        /// </summary>
        /// <param name="random">A crypto-random number generated</param>
        /// <returns>An integer value representing the serial number</returns>
        public BigInteger GenerateSerialNumber()
        {
            var random = new SecureRandom();
            return GenerateSerialNumber(random);
        }

        /// <summary>
        /// Create a serial number used for certificate and other cryptography objects
        /// </summary>
        /// <param name="random">A crypto-random number generated</param>
        /// <returns>An integer value representing the serial number</returns>
        public BigInteger GenerateSerialNumber(SecureRandom random)
        {
            var serialNumber =
                BigIntegers.CreateRandomInRange(
                    BigInteger.One, BigInteger.ValueOf(Int64.MaxValue), random);
            return serialNumber;
        }

        public byte[] GeneratePasswordHash(string plainText)
        {
            byte[] hashedPwd;
            byte[] salt = Encoding.UTF8.GetBytes(Constants.PasswordHashSalt);
            using (var rfc = new Rfc2898DeriveBytes(plainText, salt, Constants.PasswordHashIterations))
            {
                hashedPwd = rfc.GetBytes(Constants.PasswordHashLength);
            }
            return hashedPwd;
        }

        public AsymmetricCipherKeyPair LoadKeyPair(string filePath, string encryptionKey)
        {
            using (FileStream fs = File.OpenRead(filePath))
            using (TextReader tr = new StreamReader(fs))
            {
                PemReader pr = new PemReader(tr, new PasswordFinder(encryptionKey));
                var pemObject = pr.ReadObject();
                var privateKey = (RsaPrivateCrtKeyParameters)pemObject;
                var publicKey = new RsaKeyParameters(false, privateKey.Modulus, privateKey.PublicExponent);
                var keyPair = new AsymmetricCipherKeyPair(publicKey, privateKey);

                return keyPair;
            }
        }

        public string GenerateJwt(ClaimsIdentity claims, DateTime tokenExpiry)
        {
            var certificate = (X509Certificate2)X509Certificate.CreateFromCertFile(_config.TokenSettings.SigningCertificatePath);
            SigningCredentials signingCredentials = new X509SigningCredentials(certificate);
            var jwtHandler = new JwtSecurityTokenHandler();
            var tokenDesc = new SecurityTokenDescriptor
            {
                SigningCredentials = signingCredentials,
                Subject = claims,
                NotBefore = DateTime.Now,
                Expires = tokenExpiry,
                Issuer = _config.TokenSettings.Issuer,
                Audience = _config.TokenSettings.Audience
            };

            var t = jwtHandler.CreateToken(tokenDesc);
            string token = jwtHandler.WriteToken(t);

            return token;
        }

        public void SavePrivateKey(AsymmetricCipherKeyPair keyPair, string filePath, SecureRandom encryptionRandom)
        {
            var generator = new Pkcs8Generator(keyPair.Private, Pkcs8Generator.PbeSha1_3DES);
            generator.Password = _config.EncryptionKey.ToCharArray();
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

        public ClaimsPrincipal ValidateJwt(string token, X509Certificate2 signingCertificate, string issuer, string audience)
        {
            SigningCredentials signingCredentials = new X509SigningCredentials(signingCertificate);

            var handler = new JwtSecurityTokenHandler();
            var validationParams = new TokenValidationParameters
            {
                RequireSignedTokens = true,
                RequireExpirationTime = true,
                RequireAudience = true,
                ValidateTokenReplay = true,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidAudience = audience,
                ValidIssuer = issuer,
                IssuerSigningKey = signingCredentials.Key
            };

            try
            {
                SecurityToken validatedToken;
                var claims = handler.ValidateToken(token, validationParams, out validatedToken);

                return claims;
            }
            catch (SecurityTokenInvalidAudienceException invalidAudience)
            {
                _log.Error(invalidAudience.Message);
            }
            catch (SecurityTokenInvalidIssuerException invalidIssuer)
            {
                _log.Error(invalidIssuer.Message);
            }
            catch (SecurityTokenInvalidSignatureException invalidSignature)
            {
                _log.Error(invalidSignature.Message);
            }
            catch (SecurityTokenReplayDetectedException replayDetected)
            {
                _log.Error(replayDetected.Message);
            }
            catch (SecurityTokenExpiredException expiredToken)
            {
                _log.Error(expiredToken.Message);
            }

            return null;
        }

        public bool VerifyPasswordHash(byte[] passwordHash)
        {
            byte[] hashedPwd;
            byte[] salt = Encoding.UTF8.GetBytes(Constants.PasswordHashSalt);
            using (var rfc = new Rfc2898DeriveBytes(passwordHash, salt, Constants.PasswordHashIterations))
            {
                hashedPwd = rfc.GetBytes(Constants.PasswordHashLength);
            }

            return hashedPwd.AreBytesEqual(passwordHash);
        }
    }
}
