using System.ComponentModel.DataAnnotations;

namespace InvestimentosCaixa.Api.Models.Simulacao
{
    public class SimularInvestimentoModel
    {
        [Required(ErrorMessage = "O ID do cliente é obrigatório.")]
        public int ClienteId { get; set; }

        [Required(ErrorMessage = "O valor do investimento é obrigatório.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero.")]
        [DataType(DataType.Currency)]
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "O prazo em meses é obrigatório.")]
        [Range(1, 360, ErrorMessage = "O prazo deve estar entre 1 e 360 meses.")]
        public short PrazoMeses { get; set; }

        [Required(ErrorMessage = "O tipo de produto é obrigatório.")]
        [StringLength(50, ErrorMessage = "O tipo de produto não pode exceder 50 caracteres.")]
        public string TipoProduto { get; set; }
    }
}
