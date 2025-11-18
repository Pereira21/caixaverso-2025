namespace InvestimentosCaixa.Application.DTO
{
    public class ProdutoDto
    {
        public int Id { get; set; }
        public int TipoProdutoId { get; set; }
        public string Nome { get; set; }
        public decimal RentabilidadeAnual { get; set; }
        public short PrazoMinimoMeses { get; set; }
        public TipoProdutoDto TipoProduto { get; set; }
    }
}
