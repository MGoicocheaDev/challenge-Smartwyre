using Smartwyre.DeveloperTest.CustomExceptions;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services.RebateCalculator
{
    /// <summary>
    /// Calculator implementation for fixed rate rebates.
    /// </summary>
    internal class FixedRateRebateCalculator : IRebateCalculator
    {
        /// <inheritdoc/>
        /// <exception cref="RebateException">Thrown if an error occurs during rebate calculation.</exception>
        public decimal CalculateRebate(Rebate rebate, Product product, CalculateRebateRequest rebateRequest)
        {
            if ( rebate.Amount == 0 || product.Price == 0 || rebateRequest.Volume == 0 || !product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedRateRebate))
                throw new RebateException($"Invalid input parameters for {nameof(FixedRateRebateCalculator)}");


            return product.Price * rebate.Percentage * rebateRequest.Volume;
        }
    }
}
