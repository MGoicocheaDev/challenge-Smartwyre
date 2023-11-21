using Moq;
using Serilog;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;
using System;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests;

public class PaymentServiceTests
{

    [Theory]
    [InlineData(IncentiveType.AmountPerUom, SupportedIncentiveType.AmountPerUom)]
    [InlineData(IncentiveType.FixedRateRebate, SupportedIncentiveType.FixedRateRebate)]
    [InlineData(IncentiveType.FixedCashAmount, SupportedIncentiveType.FixedCashAmount)]
    public void RebateServices_should_CalculateSuccess(IncentiveType incentiveType, SupportedIncentiveType supported)
    {
        var rebateDataStoreMock = new Mock<IRebateDataStore>();
        var productDataStoreMock = new Mock<IProductDataStore>();
        var loggerMock = new Mock<ILogger>();

        var rebateServices = new RebateService(productDataStoreMock.Object, rebateDataStoreMock.Object, loggerMock.Object);

        rebateDataStoreMock.Setup(mock => mock.GetRebate(It.IsAny<string>()))
                           .Returns(new Rebate { Incentive = incentiveType, Amount = 10 });

        productDataStoreMock.Setup(mock => mock.GetProduct(It.IsAny<string>()))
                            .Returns(new Product { SupportedIncentives = supported, Price = 2 });


        var request = new CalculateRebateRequest { RebateIdentifier = "rebate1", ProductIdentifier = "product1", Volume = 5 };

        var result = rebateServices.Calculate(request);

        Assert.True(result.Success);

    }

    [Theory]
    [InlineData(SupportedIncentiveType.AmountPerUom)]
    [InlineData(SupportedIncentiveType.FixedRateRebate)]
    [InlineData(SupportedIncentiveType.FixedCashAmount)]
    public void RebateServices_should_CalculateFailForRebateStoreNull(SupportedIncentiveType supported)
    {
        var rebateDataStoreMock = new Mock<IRebateDataStore>();
        var productDataStoreMock = new Mock<IProductDataStore>();
        var loggerMock = new Mock<ILogger>();

        var rebateServices = new RebateService(productDataStoreMock.Object, rebateDataStoreMock.Object, loggerMock.Object);

        rebateDataStoreMock.Setup(mock => mock.GetRebate(It.IsAny<string>()))
                           .Returns((Rebate)null);

        productDataStoreMock.Setup(mock => mock.GetProduct(It.IsAny<string>()))
                            .Returns(new Product { SupportedIncentives = supported, Price = 2 });


        var request = new CalculateRebateRequest { RebateIdentifier = "rebate1", ProductIdentifier = "product1", Volume = 5 };
        var result = rebateServices.Calculate(request);
        Assert.False(result.Success);
    }

    [Theory]
    [InlineData(IncentiveType.AmountPerUom)]
    [InlineData(IncentiveType.FixedRateRebate)]
    [InlineData(IncentiveType.FixedCashAmount)]
    public void RebateServices_should_CalculateFailForProductStoreNull(IncentiveType incentiveType)
    {
        var rebateDataStoreMock = new Mock<IRebateDataStore>();
        var productDataStoreMock = new Mock<IProductDataStore>();
        var loggerMock = new Mock<ILogger>();

        var rebateServices = new RebateService(productDataStoreMock.Object, rebateDataStoreMock.Object, loggerMock.Object);

        rebateDataStoreMock.Setup(mock => mock.GetRebate(It.IsAny<string>()))
                           .Returns(new Rebate { Incentive = incentiveType, Amount = 10 });

        productDataStoreMock.Setup(mock => mock.GetProduct(It.IsAny<string>()))
                            .Returns((Product)null);


        var request = new CalculateRebateRequest { RebateIdentifier = "rebate1", ProductIdentifier = "product1", Volume = 5 };
        var result = rebateServices.Calculate(request);
        Assert.False(result.Success);
    }


    [Theory]
    [InlineData(IncentiveType.AmountPerUom, SupportedIncentiveType.AmountPerUom, 50)]
    [InlineData(IncentiveType.FixedRateRebate, SupportedIncentiveType.FixedRateRebate, 2)]
    [InlineData(IncentiveType.FixedCashAmount, SupportedIncentiveType.FixedCashAmount, 10)]
    public void RebateServices_should_CalculateFailForErrorInStoreCalculationResult(IncentiveType incentiveType, SupportedIncentiveType supported, decimal rebateAmount)
    {
        var rebateDataStoreMock = new Mock<IRebateDataStore>();
        var productDataStoreMock = new Mock<IProductDataStore>();
        var loggerMock = new Mock<ILogger>();

      
        var rebate = new Rebate { Incentive = incentiveType, Amount = 10, Percentage = 0.2m };
        var product = new Product { SupportedIncentives = supported, Price = 2 };

        var rebateServices = new RebateService(productDataStoreMock.Object, rebateDataStoreMock.Object, loggerMock.Object);


        rebateDataStoreMock.Setup(mock => mock.GetRebate(It.IsAny<string>()))
                           .Returns(rebate);

        productDataStoreMock.Setup(mock => mock.GetProduct(It.IsAny<string>()))
                            .Returns(product);

        rebateDataStoreMock.Setup(mock => mock.StoreCalculationResult(rebate, rebateAmount)).Throws<Exception>();


        var request = new CalculateRebateRequest { RebateIdentifier = "rebate1", ProductIdentifier = "product1", Volume = 5 };

        var result = rebateServices.Calculate(request);
        rebateDataStoreMock.Verify(mock => mock.StoreCalculationResult(It.IsAny<Rebate>(), It.IsAny<decimal>()), Times.Once);
        Assert.False(result.Success);

    }

}
