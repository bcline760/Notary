using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Notary.Contract;

namespace Notary.Interface.Service
{
    /// <summary>
    /// Service which handles API tokens.
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Get a list of tokens by the account.
        /// </summary>
        /// <param name="slug"></param>
        /// <returns></returns>
        Task<List<ApiToken>> GetAccountTokens(string slug);

        /// <summary>
        /// Save a token to the data store.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task SaveAsync(ApiToken token);
    }
}
