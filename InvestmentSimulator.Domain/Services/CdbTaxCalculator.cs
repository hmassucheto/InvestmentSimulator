using InvestmentSimulator.Domain.Interfaces;

namespace InvestmentSimulator.Domain.Services;
public class CdbTaxCalculator : ITaxCalculator {
    public decimal CalculateTax(decimal initialValue, decimal finalValue, int months) {
        var profit = finalValue - initialValue;
        var rate = GetTaxRate(months);
        return Math.Round(profit * rate, 2, MidpointRounding.AwayFromZero);
    }

    private static decimal GetTaxRate(int months) {
        if (months <= 6) return 0.225m;
        if (months <= 12) return 0.20m;
        if (months <= 24) return 0.175m;
        return 0.15m;
    }
}
