using System.ComponentModel.DataAnnotations;

namespace InvestimentosCaixa.Api.Models
{
    public class SimularInvestimentoModel
    {
        //[Required(ErrorMessage = "A quantidade de meses é obrigatória.")]
        public int ClienteId { get; set; }

        //[Required(ErrorMessage = "O valor do empréstimo é obrigatório.")]
        [DataType(DataType.Currency)]
        public decimal Valor { get; set; }

        //[Required(ErrorMessage = "A quantidade de meses é obrigatória.")]
        public short PrazoMeses { get; set; }

        public string TipoProduto { get; set; }
    }
}
