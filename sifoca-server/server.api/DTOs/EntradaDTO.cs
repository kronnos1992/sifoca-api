using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace server.api.DTOs
{
    public class EntradaDTO
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

        [Required(ErrorMessage = "o campo Area é obrigatórion")]
        [StringLength(
            maximumLength: 100,
            ErrorMessage = "o campo Area requer no máximo 50 caracters",
            MinimumLength = 2,
            ErrorMessageResourceName = "o campo Area requer no minimo 5 caracters")
        ]
        public string? Area { get; set; }
        public string TipoPagamento { get; set; }
        public string Assinante { get; set; } = "Particular";
    }
}