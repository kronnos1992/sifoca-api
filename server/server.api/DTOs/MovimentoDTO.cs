
using System.ComponentModel.DataAnnotations;


namespace server.api.DTOs
{
    public class MovimentoDTO
    {
        [Required(ErrorMessage = "o campo categoria é obrigatórion")]
        [StringLength(
            maximumLength: 100,
            ErrorMessage = "o campo categoria requer no máximo 50 caracters",
            MinimumLength = 5,
            ErrorMessageResourceName = "o campo categoria requer no minimo 5 caracters")
        ]
        public string? Descricao { get; set; }


        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "o campo Descrição é obrigatórion")]
        public decimal Valor { get; set; }


        [Required(ErrorMessage = "o campo Operador é obrigatórion")]
        [StringLength(
            maximumLength: 100,
            ErrorMessage = "o campo Operador requer no máximo 50 caracters",
            MinimumLength = 2,
            ErrorMessageResourceName = "o campo Operador requer no minimo 5 caracters")
        ]
        public string? Operador { get; set; }
    }
}