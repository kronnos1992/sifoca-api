
using System.ComponentModel.DataAnnotations;


namespace server.api.DTOs
{
    public class MovimentoDTO
    {
        [Required(ErrorMessage = "o campo categoria é obrigatório")]
        [StringLength(maximumLength: 100)]
        public string? Descricao { get; set; }

        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "o campo Descrição é obrigatório")]
        public decimal Valor { get; set; }


        [Required(ErrorMessage = "o campo Operador é obrigatório")]
        [StringLength(maximumLength: 100)]
        public string? Beneficiario { get; set; }
    }
}