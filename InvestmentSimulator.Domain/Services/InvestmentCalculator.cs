using InvestmentSimulator.Api.Models;
using InvestmentSimulator.Domain.Configuration;
using Microsoft.Extensions.Options;

namespace InvestmentSimulator.Domain.Interfaces;
public class InvestmentCalculator(ITaxCalculator taxCalculator, IOptions<InvestmentSettings> options) : IInvestmentCalculator {
    private readonly ITaxCalculator _taxCalculatorService = taxCalculator;
    private readonly InvestmentSettings _investmentSettings = options.Value;

    public InvestmentResult Calculate(decimal initialValue, int months) {
        var amount = initialValue;

        for (var i = 0; i < months; i++) {
            amount *= (1 + (_investmentSettings.Cdi * _investmentSettings.Tb));
        }

        var gross = Math.Round(amount, 2, MidpointRounding.AwayFromZero);
        var tax = _taxCalculatorService.CalculateTax(initialValue, gross, months);
        var net = Math.Round(gross - tax, 2, MidpointRounding.AwayFromZero);

        return new InvestmentResult {
            GrossAmount = gross,
            NetAmount = net
        };
    }
}
