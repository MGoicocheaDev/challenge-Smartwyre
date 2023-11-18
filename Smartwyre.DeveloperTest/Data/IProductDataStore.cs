using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Data
{
    /// <summary>
    /// 
    /// </summary>
    public interface IProductDataStore
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="productIdentifier"></param>
        /// <returns></returns>
        Product GetProduct(string productIdentifier);
    }
}
