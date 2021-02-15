using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Notary.Contract;

namespace Notary.Interface.Service
{
    /// <summary>
    /// Service which handles API tokens.
    /// </summary>
    public interface ITokenService : IEntityService<AuthenticatedUser>
    {
        /// <summary>
        /// Get a list of tokens by the account.
        /// </summary>
        /// <param name="slug"></param>
        /// <returns></returns>
        Task<List<AuthenticatedUser>> GetAccountTokens(string slug);
    }
}
