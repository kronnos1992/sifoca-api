
using System.ComponentModel.DataAnnotations;
namespace server.api.DTOs;

public class SaidaDTO
{
    [Required(ErrorMessage = "o campo descrição é obrigatório")]
    [StringLength(maximumLength: 100)]
    public string ? DescricaoSaida { get; set; }

    [DataType(DataType.Currency)]
    [Required(ErrorMessage = "o campo valor é obrigatório")]
    public decimal ValorSaida { get; set; }
    
    [Required(ErrorMessage = "o campo beneficiario é obrigatório")]
    [StringLength(
        maximumLength: 100,
        MinimumLength = 4)
    ]
    public required string Beneficiario { get; set; }
    
}