using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Notary.Contract;

namespace Notary.Interface.Service
{
    public interface ISessionService
    {
        /// <summary>
        /// Authenticate for an API token
        /// </summary>
        /// <param name="credentials">The type of credentials to sign on with.</param>
        /// <returns></returns>
        Task<ApiToken> SignInAsync(ICredentials credentials);

        /// <summary>
        /// Sign out and negate active API token
        /// </summary>
        /// <param name="accountSlug"></param>
        /// <returns></returns>
        Task SignoutAsync(string accountSlug);
    }
}
