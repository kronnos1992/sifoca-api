
using System.ComponentModel.DataAnnotations;


namespace server.api.DTOs;

public class EntradaDTO
{
    [Required(ErrorMessage = "o campo categoria é obrigatório")]
    [StringLength(maximumLength: 100)]
    public string ? DescricaoEntrada { get; set; }

    [DataType(DataType.Currency)]
    [Required(ErrorMessage = "o campo Descrição é obrigatório")]
    public decimal ValorEntrada { get; set; }


    [Required(ErrorMessage = "o campo Operador é obrigatório")]
    [StringLength(maximumLength: 100)]
    public required string FonteEntrada { get; set; }
    
    [Required(ErrorMessage = "o campo forma de pagamento é obrigatório")]
    [StringLength(
        maximumLength: 100,
        MinimumLength = 3)
    ]
    public required string FormaPagamento { get; set; }

}