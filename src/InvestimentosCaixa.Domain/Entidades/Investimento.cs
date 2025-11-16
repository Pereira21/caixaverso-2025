namespace InvestimentosCaixa.Domain.Entidades
{
    public class Investimento : BaseEntity
    {
        public int ClienteId { get; set; }
        public int ProdutoId { get; set; }        
        public decimal Valor { get; set; }
        public decimal Rentabilidade { get; set; }
        public DateTime Data { get; set; }

        public Cliente Cliente { get; set; }
        public Produto Produto { get; set; }
    }
}
