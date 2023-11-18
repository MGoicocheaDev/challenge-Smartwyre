using Smartwyre.DeveloperTest.CustomExceptions;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services.RebateCalculator
{
    /// <summary>
    /// Calculator implementation for rebates based on an amount per unit of measure.
    /// </summary>
    internal class AmountPerUomRebateCalculator : IRebateCalculator
    {
        /// <inheritdoc/>
        /// <exception cref="RebateException">Thrown if an error occurs during rebate calculation.</exception>
        public decimal CalculateRebate(Rebate rebate, Product product, CalculateRebateRequest rebateRequest)
        {
            if (rebate.Amount == 0 || rebateRequest.Volume == 0 || !product.SupportedIncentives.HasFlag(SupportedIncentiveType.AmountPerUom))
                throw new RebateException($"Invalid input parameters for {nameof(AmountPerUomRebateCalculator)}");

            return rebate.Amount * rebateRequest.Volume;
        }
    }
}
