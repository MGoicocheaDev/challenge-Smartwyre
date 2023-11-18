using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Data
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRebateDataStore
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rebateIdentifier"></param>
        /// <returns></returns>
        Rebate GetRebate(string rebateIdentifier);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="account"></param>
        /// <param name="rebateAmount"></param>
        void StoreCalculationResult(Rebate account, decimal rebateAmount);
    }
}
