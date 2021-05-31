using System.Threading.Tasks;

namespace Notary.Interface
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICollectionSetup
    {
        /// <summary>
        /// Run the collection setup
        /// </summary>
        /// <returns></returns>
        Task Setup();
    }
}