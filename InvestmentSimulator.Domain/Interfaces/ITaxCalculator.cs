namespace InvestmentSimulator.Domain.Interfaces;

public interface ITaxCalculator {
    decimal CalculateTax(decimal initialValue, decimal finalValue, int months);
}
