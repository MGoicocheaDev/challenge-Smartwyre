using Smartwyre.DeveloperTest.CustomExceptions;
using Smartwyre.DeveloperTest.Services.RebateCalculator;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests
{
    public class RebateCalculatorTests
    {

        [Fact]
        public void CalculateAmountPerUom_Should_ReturnAmount()
        {
            IRebateCalculator _amountPerUomRebateCalculator = new AmountPerUomRebateCalculator();
            var request = new CalculateRebateRequest
            {
                RebateIdentifier = "AmountPerUom",
                ProductIdentifier = "Product-1",
                Volume = 2
            };
            var rebate = new Rebate { Incentive = IncentiveType.AmountPerUom, Amount = 1, Identifier = "AmountPerUom", Percentage = 2 };
            var product = new Product { Id = 1, Price = 2, Uom = "Uom-1", Identifier = "Product-1", SupportedIncentives = SupportedIncentiveType.AmountPerUom };

            var result = _amountPerUomRebateCalculator.CalculateRebate(rebate, product, request);
            Assert.Equal(2, result);
        }

        [Theory]
        [InlineData(0, 0, SupportedIncentiveType.FixedRateRebate)]
        [InlineData(1, 0, SupportedIncentiveType.FixedRateRebate)]
        [InlineData(0, 1, SupportedIncentiveType.FixedRateRebate)]
        [InlineData(0, 0, SupportedIncentiveType.FixedCashAmount)]
        [InlineData(1, 0, SupportedIncentiveType.FixedCashAmount)]
        [InlineData(0, 1, SupportedIncentiveType.FixedCashAmount)]
        public void CalculateAmountPerUom_Should_ReturnRebateException(decimal amount, decimal volume, SupportedIncentiveType supported)
        {
            IRebateCalculator _amountPerUomRebateCalculator = new AmountPerUomRebateCalculator();
            var request = new CalculateRebateRequest
            {
                RebateIdentifier = "AmountPerUom",
                ProductIdentifier = "Product-1",
                Volume = volume
            };
            var rebate = new Rebate { Incentive = IncentiveType.AmountPerUom, Amount = amount, Identifier = "AmountPerUom", Percentage = 2 };
            var product = new Product { Id = 1, Price = 2, Uom = "Uom-1", Identifier = "Product-1", SupportedIncentives = supported };

            Assert.Throws<RebateException>(() => _amountPerUomRebateCalculator.CalculateRebate(rebate, product, request));
        }

        [Fact]
        public void FixedCashAmount_Should_ReturnAmount()
        {
            IRebateCalculator _fixedCashAmountRebateCalculator = new FixedCashAmountRebateCalculator();
            var request = new CalculateRebateRequest
            {
                RebateIdentifier = "FixedCashAmount",
                ProductIdentifier = "Product-1",
                Volume = 2
            };
            var rebate = new Rebate { Incentive = IncentiveType.FixedCashAmount, Amount = 1, Identifier = "FixedCashAmount", Percentage = 2 };
            var product = new Product { Id = 1, Price = 2, Uom = "Uom-1", Identifier = "Product-1", SupportedIncentives = SupportedIncentiveType.FixedCashAmount };

            var result = _fixedCashAmountRebateCalculator.CalculateRebate(rebate, product, request);
            Assert.Equal(1, result);
        }

        [Theory]
        [InlineData(0, SupportedIncentiveType.FixedRateRebate)]
        [InlineData(1, SupportedIncentiveType.FixedRateRebate)]
        [InlineData(0, SupportedIncentiveType.AmountPerUom)]
        [InlineData(1, SupportedIncentiveType.AmountPerUom)]
        public void FixedCashAmount_Should_ReturnRebateException(decimal amount, SupportedIncentiveType supported)
        {
            IRebateCalculator _fixedCashAmountRebateCalculator = new FixedCashAmountRebateCalculator();
            var request = new CalculateRebateRequest
            {
                RebateIdentifier = "FixedCashAmount",
                ProductIdentifier = "Product-1",
                Volume = 2
            };
            var rebate = new Rebate { Incentive = IncentiveType.FixedCashAmount, Amount = amount, Identifier = "FixedCashAmount", Percentage = 2 };
            var product = new Product { Id = 1, Price = 2, Uom = "Uom-1", Identifier = "Product-1", SupportedIncentives = supported };

            Assert.Throws<RebateException>(() => _fixedCashAmountRebateCalculator.CalculateRebate(rebate, product, request));
        }

        [Fact]
        public void FixedRateAmount_Should_ReturnAmount()
        {
            IRebateCalculator _fixedRateRebateCalculator = new FixedRateRebateCalculator();
            var request = new CalculateRebateRequest
            {
                RebateIdentifier = "FixedRateRebate",
                ProductIdentifier = "Product-1",
                Volume = 2
            };
            var rebate = new Rebate { Incentive = IncentiveType.FixedRateRebate, Amount = 1, Identifier = "FixedRateRebate", Percentage = 0.1m };
            var product = new Product { Id = 1, Price = 2, Uom = "Uom-1", Identifier = "Product-1", SupportedIncentives = SupportedIncentiveType.FixedRateRebate };

            var result = _fixedRateRebateCalculator.CalculateRebate(rebate, product, request);
            Assert.Equal(0.4m, result);
        }

        [Theory]
        [InlineData(0, 0, 0, SupportedIncentiveType.FixedCashAmount)]
        [InlineData(0, 1, 0, SupportedIncentiveType.FixedCashAmount)]
        [InlineData(0, 0, 1, SupportedIncentiveType.FixedCashAmount)]
        [InlineData(1, 1, 1, SupportedIncentiveType.FixedCashAmount)]
        [InlineData(0, 0, 0, SupportedIncentiveType.AmountPerUom)]
        [InlineData(0, 1, 0, SupportedIncentiveType.AmountPerUom)]
        [InlineData(0, 0, 1, SupportedIncentiveType.AmountPerUom)]
        [InlineData(1, 1, 1, SupportedIncentiveType.AmountPerUom)]
        public void FixedRateAmount_Should_ReturnRebateException(decimal amount, decimal percentage, decimal volume, SupportedIncentiveType supported)
        {
            IRebateCalculator _fixedRateRebateCalculator = new FixedRateRebateCalculator();
            var request = new CalculateRebateRequest
            {
                RebateIdentifier = "FixedRateRebate",
                ProductIdentifier = "Product-1",
                Volume = volume
            };
            var rebate = new Rebate { Incentive = IncentiveType.FixedRateRebate, Amount = amount, Identifier = "FixedRateRebate", Percentage = percentage };
            var product = new Product { Id = 1, Price = 2, Uom = "Uom-1", Identifier = "Product-1", SupportedIncentives = supported };

            Assert.Throws<RebateException>(() => _fixedRateRebateCalculator.CalculateRebate(rebate, product, request));
        }
    }
}
