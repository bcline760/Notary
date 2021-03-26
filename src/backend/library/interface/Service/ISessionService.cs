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
        Task<AuthenticatedUser> SignInAsync(ICredentials credentials);

        /// <summary>
        /// Sign out and negate active API token
        /// </summary>
        /// <param name="accountSlug"></param>
        /// <returns></returns>
        Task SignoutAsync(string accountSlug);

        /// <summary>
        /// Get an account's active sessions
        /// </summary>
        /// <param name="accountSlug">The slug of the account</param>
        /// <param name="activeOnly">Whether to return only the active sessions of the account</param>
        /// <returns>List of sessions matching the account and parameters</returns>
        Task<List<AuthenticatedUser>> GetSessions(string accountSlug, bool activeOnly);
    }
}
