using InvestmentSimulator.Domain.Services;

namespace InvestmentSimulator.Tests;
public class CdbTaxCalculatorTests {
    private readonly CdbTaxCalculator _cdbTaxCalculator = new();

    [Theory]
    [InlineData(1000, 1100, 6, 0.225)]
    [InlineData(1000, 1100, 12, 0.20)]
    [InlineData(1000, 1100, 24, 0.175)]
    [InlineData(1000, 1100, 25, 0.15)]
    public void ShouldCalculateTaxBasedOnRate(decimal vi, decimal vf, int months, decimal expectedRate) {
        var profit = vf - vi;
        var expectedTax = Math.Round(profit * expectedRate, 2);

        var actualTax = _cdbTaxCalculator.CalculateTax(vi, vf, months);

        Assert.Equal(expectedTax, actualTax);
    }

    [Fact]
    public void ShouldReturnZeroWhenThereIsNoProfit() {
        var tax = _cdbTaxCalculator.CalculateTax(1000, 1000, 12);
        Assert.Equal(0, tax);
    }
}
