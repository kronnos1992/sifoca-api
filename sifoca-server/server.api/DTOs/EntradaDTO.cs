using System.ComponentModel.DataAnnotations;

namespace server.api.DTOs
{
    public class EntradaDTO
    {
        [Required(ErrorMessage = "o campo descrição é obrigatório")]
        [StringLength(
            maximumLength: 100,
            MinimumLength = 5)
        ]
        public string? Descricao { get; set; }
        [DataType(DataType.Currency, ErrorMessage ="o campo valor, so aceita numeros")]
        [Required(ErrorMessage = "o campo valor é obrigatórion")]
        
        public decimal Valor { get; set; }
        [Required(ErrorMessage = "o campo tipo de pagamento é obrigatório")]
        [StringLength(
            maximumLength: 100,
            MinimumLength = 5)
        ]
        public required string TipoPagamento { get; set; }

        [Required(ErrorMessage = "o campo forma de pagamento é obrigatório")]
        [StringLength(
            maximumLength: 100,
            MinimumLength = 3)
        ]
        public required string FormaPagamento { get; set; }

        [Required(ErrorMessage = "o campo assinante é obrigatório")]
        [StringLength(
            maximumLength: 100,
            MinimumLength = 5)
        ]
        public string Assinante { get; set; } = "Particular";
    }
}