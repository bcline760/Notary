using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Notary.Contract;

namespace Notary.Interface.Repository
{
    public interface ITokenRepository : IRepository<AuthenticatedUser>
    {
        /// <summary>
        /// Get all tokens by the account slug
        /// </summary>
        /// <param name="accountSlug"></param>
        /// <returns>List of tokens matching the account slug</returns>
        Task<List<AuthenticatedUser>> GetAccountTokens(string accountSlug);
    }
}
