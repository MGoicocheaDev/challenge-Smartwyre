using Smartwyre.DeveloperTest.CustomExceptions;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services.RebateCalculator;
using Smartwyre.DeveloperTest.Types;
using System;
using System.Collections.Generic;

namespace Smartwyre.DeveloperTest.Services;

public class RebateService : IRebateService
{
    private readonly IProductDataStore _productDataStore;
    private readonly IRebateDataStore _rebateDataStore;
    private readonly Dictionary<IncentiveType, IRebateCalculator> _rebateCalculatorStrategy;

    /// <summary>
    /// Initializes a new instance of the <see cref="RebateService"/> class.
    /// </summary>
    /// <param name="productDataStore"> The data store for product information </param>
    /// <param name="rebateDataStore"> The data store for rebate information </param>
    public RebateService( IProductDataStore productDataStore, IRebateDataStore rebateDataStore)
    {
        _productDataStore = productDataStore;
        _rebateDataStore = rebateDataStore;
        _rebateCalculatorStrategy = new Dictionary<IncentiveType, IRebateCalculator> {
            { IncentiveType.AmountPerUom, new AmountPerUomRebateCalculator() },
            { IncentiveType.FixedCashAmount, new FixedCashAmountRebateCalculator() },
            { IncentiveType.FixedRateRebate, new FixedRateRebateCalculator() },
        };
    }

    /// <summary>
    /// Calculates the rebate amount based on the provided request.
    /// </summary>
    /// <param name="request"> The request containing rebate and product information </param>
    /// <returns> A <see cref="CalculateRebateResult"/> indicating the success of the calculation and the calculated rebate amount </returns>

    public CalculateRebateResult Calculate(CalculateRebateRequest request)
    {
        Rebate rebate = _rebateDataStore.GetRebate(request.RebateIdentifier);
        Product product = _productDataStore.GetProduct(request.ProductIdentifier);

        var result = new CalculateRebateResult();

        try
        {
            if (rebate == null) throw new RebateException("Rebate data is invalid");
            if (product == null) throw new RebateException("Product data is invalid");
            if (request == null) throw new RebateException("CalculateRebateRequest data is invalid");

            decimal rebateResult = _rebateCalculatorStrategy[rebate.Incentive].CalculateRebate(rebate, product, request);
            _rebateDataStore.StoreCalculationResult(rebate, rebateResult);
            result.Success = true;
        }
        catch (Exception ex)
        {
            result.Success = false;
            /// Write the exception in log
            Console.WriteLine(ex.ToString());
        }
        return result;
    }

    [Obsolete]
    public CalculateRebateResult Calculate_Obsolete(CalculateRebateRequest request)
    {
        var rebateDataStore = new RebateDataStore();
        var productDataStore = new ProductDataStore();

        Rebate rebate = rebateDataStore.GetRebate(request.RebateIdentifier);
        Product product = productDataStore.GetProduct(request.ProductIdentifier);

        var result = new CalculateRebateResult();

        var rebateAmount = 0m;

        switch (rebate.Incentive)
        {
            case IncentiveType.FixedCashAmount:
                if (rebate == null)
                {
                    result.Success = false;
                }
                else if (!product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedCashAmount))
                {
                    result.Success = false;
                }
                else if (rebate.Amount == 0)
                {
                    result.Success = false;
                }
                else
                {
                    rebateAmount = rebate.Amount;
                    result.Success = true;
                }
                break;

            case IncentiveType.FixedRateRebate:
                if (rebate == null)
                {
                    result.Success = false;
                }
                else if (product == null)
                {
                    result.Success = false;
                }
                else if (!product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedRateRebate))
                {
                    result.Success = false;
                }
                else if (rebate.Percentage == 0 || product.Price == 0 || request.Volume == 0)
                {
                    result.Success = false;
                }
                else
                {
                    rebateAmount += product.Price * rebate.Percentage * request.Volume;
                    result.Success = true;
                }
                break;

            case IncentiveType.AmountPerUom:
                if (rebate == null)
                {
                    result.Success = false;
                }
                else if (product == null)
                {
                    result.Success = false;
                }
                else if (!product.SupportedIncentives.HasFlag(SupportedIncentiveType.AmountPerUom))
                {
                    result.Success = false;
                }
                else if (rebate.Amount == 0 || request.Volume == 0)
                {
                    result.Success = false;
                }
                else
                {
                    rebateAmount += rebate.Amount * request.Volume;
                    result.Success = true;
                }
                break;
        }

        if (result.Success)
        {
            var storeRebateDataStore = new RebateDataStore();
            storeRebateDataStore.StoreCalculationResult(rebate, rebateAmount);
        }

        return result;
    }
}
