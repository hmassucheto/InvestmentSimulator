using InvestmentSimulator.Api.Models;
using InvestmentSimulator.Domain.Configuration;
using InvestmentSimulator.Domain.Interfaces;
using Moq;

namespace InvestmentSimulator.Tests;

public class InvestmentCalculatorTests {
#pragma warning disable CA1859 // CA1859: intentional use of abstraction
    private readonly IInvestmentCalculator _investmentCalculator;
#pragma warning restore CA1859

    public InvestmentCalculatorTests() {
        var investmentSettings = new InvestmentSettings {
            Cdi = 0.009m,
            Tb = 1.08m
        };

        var taxCalculatorMock = new Mock<ITaxCalculator>();
        taxCalculatorMock.Setup(x => x.CalculateTax(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<int>()))
                         .Returns<decimal, decimal, int>((vi, vf, m) => {
                             var profit = vf - vi;

                             decimal rate;
                             if (m <= 6) rate = 0.225m;
                             else if (m <= 12) rate = 0.20m;
                             else if (m <= 24) rate = 0.175m;
                             else rate = 0.15m;

                             return Math.Round(profit * rate, 2);
                         });

        _investmentCalculator = new InvestmentCalculator(taxCalculatorMock.Object, Microsoft.Extensions.Options.Options.Create(investmentSettings));
    }

    [Fact]
    public void ShouldCalculateInvestmentCorrectly_For6Months() {
        InvestmentResult result = _investmentCalculator.Calculate(1000, 6);

        var vi = 1000m;
        var tb = 1.08m;
        var cdi = 0.009m;

        var gross = vi;
        for (var i = 0; i < 6; i++) {
            gross *= (1 + (cdi * tb));
        }
        gross = Math.Round(gross, 2);

        var tax = Math.Round((gross - vi) * 0.225m, 2);
        var net = Math.Round(gross - tax, 2);

        Assert.Equal(gross, result.GrossAmount);
        Assert.Equal(net, result.NetAmount);
    }

    [Fact]
    public void ShouldApplyCorrectTaxRate_For24Months() {
        InvestmentResult result = _investmentCalculator.Calculate(1000, 24);

        var gross = result.GrossAmount;
        var tax = Math.Round((gross - 1000) * 0.175m, 2);
        var expectedNet = Math.Round(gross - tax, 2);

        Assert.Equal(expectedNet, result.NetAmount);
        Assert.Equal(gross, result.GrossAmount);
    }

    [Fact]
    public void ShouldApplyLowestTaxRate_WhenPeriodExceeds24Months() {
        InvestmentResult result = _investmentCalculator.Calculate(1000, 25);

        var gross = result.GrossAmount;
        var tax = Math.Round((gross - 1000) * 0.15m, 2);
        var expectedNet = Math.Round(gross - tax, 2);

        Assert.Equal(expectedNet, result.NetAmount);
        Assert.Equal(gross, result.GrossAmount);
    }
}
