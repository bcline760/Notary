using Notary.Contract;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Notary.Interface.Service
{
    /// <summary>
    /// Defines means to store and configure certificates
    /// </summary>
    public interface ICertificateService : IEntityService<Certificate>
    {
        /// <summary>
        /// Creates new certificate
        /// </summary>
        /// <param name="certificate"></param>
        /// <returns></returns>
        Task IssueCertificateAsync(CertificateRequest request);

        /// <summary>
        /// Download a certificate from the CA in various formats
        /// </summary>
        /// <param name="thumbprint">The thumbprint</param>
        /// <param name="format">The format to download</param>
        /// <returns>The certificate binary data</returns>
        Task<byte[]> DownloadCertificateAsync(string slug, CertificateFormat format, string privateKeyPassword);

        /// <summary>
        /// Get a list of all revocated certificates
        /// </summary>
        /// <returns>A list of all certificates revocated</returns>
        Task<List<RevocatedCertificate>> GetRevocatedCertificates();

        /// <summary>
        /// Get the certificate used to sign requests and certificates
        /// </summary>
        /// <returns>The signing certificate</returns>
        Task<Certificate> GetSigningCertificateAsync();

        /// <summary>
        /// Revoke a certificate
        /// </summary>
        /// <param name="thumbprint">The certificate thumbprint</param>
        /// <param name="reason">The reason for its revocation</param>
        Task RevokeCertificateAsync(string slug, RevocationReason reason, string userRevocatingSlug);
    }
}
