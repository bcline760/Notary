using Notary.Contract;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Notary.Interface.Repository
{
    public interface ICertificateRepository : IRepository<Certificate>
    {
        /// <summary>
        /// Gets the signing certificate used to sign certificates
        /// </summary>
        /// <returns>The signing certificate or null if it does not exist.</returns>
        Task<Certificate> GetSigningCertificateAsync();
    }
}
