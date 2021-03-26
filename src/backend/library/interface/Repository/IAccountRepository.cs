using Notary.Contract;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Notary.Interface.Repository
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAccountRepository : IRepository<Account>
    {
        /// <summary>
        /// Get an account by the e-mail address
        /// </summary>
        /// <param name="email">The e-mail to look for</param>
        /// <returns>The account matching the request</returns>
        Task<Account> GetByEmailAddressAsync(string email);
    }
}
