namespace InvestimentosCaixa.Api.Models.Produto
{
    public class ProdutoDisponivelModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public decimal RentabilidadeAnual { get; set; }
        public short PrazoMinimoMeses { get; set; }
    }
}
