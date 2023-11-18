using Smartwyre.DeveloperTest.CustomExceptions;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services.RebateCalculator
{
    /// <summary>
    /// Calculator implementation for fixed cash amount rebates.
    /// </summary>
    internal class FixedCashAmountRebateCalculator : IRebateCalculator
    {
        /// <inheritdoc/>
        /// <exception cref="RebateException">Thrown if an error occurs during rebate calculation.</exception>
        public decimal CalculateRebate(Rebate rebate, Product product, CalculateRebateRequest rebateRequest)
        {
            if (rebate.Amount == 0 || !product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedCashAmount))
                throw new RebateException($"Invalid input parameters for {nameof(FixedCashAmountRebateCalculator)}"); 

            return rebate.Amount;
        }
    }
}
