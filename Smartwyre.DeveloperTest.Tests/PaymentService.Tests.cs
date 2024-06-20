using System;
using Moq;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests;

public class PaymentServiceTests
{
    [Fact]
    public void Rebate_calculation_throws_when_input_is_null()
    {
        //arrange
        var productDataStore = new Mock<IProductDataStore>();
        var rebateDataStore = new Mock<IRebateDataStore>();
        var sut = new RebateService(rebateDataStore.Object, productDataStore.Object);

        //act & assert
        Assert.Throws<ArgumentNullException>(() => sut.Calculate(null));
    }

    [Fact]
    public void Rebate_calculation_fails_when_input_values_are_invalid()
    {
        //arrange
        var productDataStore = new Mock<IProductDataStore>();
        var rebateDataStore = new Mock<IRebateDataStore>();
        var sut = new RebateService(rebateDataStore.Object, productDataStore.Object);
        var request = new RebateCalculationRequest();

        //act
        var result = sut.Calculate(request);

        //assert
        Assert.False(result.Success);
    }

    [InlineData(10)]
    [InlineData(5.5)]
    [Theory]
    public void Rebate_calculation_returns_rebate_amount_when_fixed_cash_amount_incentive(decimal amount)
    {
        //arrange

        var product = new Product
        {
            SupportedIncentives = SupportedIncentiveType.FixedCashAmount
        };

        var productDataStore = new Mock<IProductDataStore>();
        productDataStore.Setup(x => x.GetProduct(It.IsAny<string>())).Returns(product);

        var rebate = new Rebate { Amount = amount, Incentive = IncentiveType.FixedCashAmount };

        var rebateDataStore = new Mock<IRebateDataStore>();
        rebateDataStore.Setup(x => x.GetRebate(It.IsAny<string>())).Returns(rebate);

        var sut = new RebateService(rebateDataStore.Object, productDataStore.Object);
        var request = new RebateCalculationRequest
        {
            RebateIdentifier = Guid.NewGuid().ToString()
        };

        //act
        var result = sut.Calculate(request);

        //assert
        Assert.True(result.Success);
        Assert.Equal(amount, result.Amount);
    }

    [InlineData(10, 5.5, 2, 110)]
    [InlineData(1, 1, 100, 100)]
    [InlineData(93, 10.85, 5.3, 5_347.965)]
    [Theory]
    public void Rebate_calculation_returns_amount_based_on_rate_on_fixed_rate_incentive(decimal volume, decimal price,
        decimal percentage, decimal expectedAmount)
    {
        //arrange

        var product = new Product
        {
            SupportedIncentives = SupportedIncentiveType.FixedRateRebate,
            Price = price
        };

        var productDataStore = new Mock<IProductDataStore>();
        productDataStore.Setup(x => x.GetProduct(It.IsAny<string>())).Returns(product);

        var rebate = new Rebate { Percentage = percentage, Incentive = IncentiveType.FixedRateRebate };

        var rebateDataStore = new Mock<IRebateDataStore>();
        rebateDataStore.Setup(x => x.GetRebate(It.IsAny<string>())).Returns(rebate);

        var sut = new RebateService(rebateDataStore.Object, productDataStore.Object);
        var request = new RebateCalculationRequest
        {
            Volume = volume
        };

        //act
        var result = sut.Calculate(request);

        //assert
        Assert.True(result.Success);
        Assert.Equal(expectedAmount, result.Amount);
    }

    [InlineData(10, 5.5, 55)]
    [InlineData(72, 10.95, 788.4)]
    [InlineData(93, 10.85, 1_009.05)]
    [Theory]
    public void Rebate_calculation_returns_amount_based_on_amount_per_uom_on_amount_per_uom_incentive(decimal volume,
        decimal amount,
        decimal expectedAmount)
    {
        //arrange

        var product = new Product
        {
            SupportedIncentives = SupportedIncentiveType.AmountPerUom
        };

        var productDataStore = new Mock<IProductDataStore>();
        productDataStore.Setup(x => x.GetProduct(It.IsAny<string>())).Returns(product);

        var rebate = new Rebate { Incentive = IncentiveType.AmountPerUom, Amount = amount };

        var rebateDataStore = new Mock<IRebateDataStore>();
        rebateDataStore.Setup(x => x.GetRebate(It.IsAny<string>())).Returns(rebate);

        var sut = new RebateService(rebateDataStore.Object, productDataStore.Object);
        var request = new RebateCalculationRequest
        {
            Volume = volume
        };

        //act
        var result = sut.Calculate(request);

        //assert
        Assert.True(result.Success);
        Assert.Equal(expectedAmount, result.Amount);
    }
}