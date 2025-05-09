using InvestmentSimulator.Api.Models;

namespace InvestmentSimulator.Domain.Interfaces {
    public interface IInvestmentCalculator {
        InvestmentResult Calculate(decimal initialValue, int months);
    }
}
