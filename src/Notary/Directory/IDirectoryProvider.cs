using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Notary.Directory
{
    public interface IDirectoryProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<bool> ValidateUserAsync(string username, string password);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<DirectoryEntity> GetDirectoryEntityAsync();
    }
}
