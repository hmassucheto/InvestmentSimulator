using System.ComponentModel.DataAnnotations;

namespace InvestmentSimulator.Api.Models;
public class InvestmentRequest {
    [Required(ErrorMessage = "O valor inicial é obrigatório.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Valor inicial deve ser maior que zero.")]
    public decimal InitialValue { get; set; }

    [Required(ErrorMessage = "O prazo é obrigatório.")]
    [Range(2, int.MaxValue, ErrorMessage = "Prazo mínimo de 2 meses.")]
    public int Months { get; set; }
}
