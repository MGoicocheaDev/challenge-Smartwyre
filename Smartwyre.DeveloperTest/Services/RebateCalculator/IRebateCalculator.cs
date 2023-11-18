using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services.RebateCalculator
{
    /// <summary>
    /// Represents a calculator for calculating rebate amounts based on specific incentive types.
    /// </summary>
    internal interface IRebateCalculator
    {
        /// <summary>
        /// Calculates the rebate amount based on the provided information.
        /// </summary>
        /// <param name="rebate"> The  <see cref="Rebate"/> information </param>
        /// <param name="product"> The  <see cref="Product"/> information</param>
        /// <param name="rebateRequest"> The <see cref="CalculateRebateRequest"/> information </param>
        /// <returns> The calculated rebate amount </returns>
        decimal CalculateRebate(Rebate rebate, Product product, CalculateRebateRequest rebateRequest);
    }
}
