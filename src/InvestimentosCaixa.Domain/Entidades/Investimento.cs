namespace InvestimentosCaixa.Domain.Entidades
{
    public class Investimento : BaseEntity
    {
        public Investimento() { }

        public Investimento(int id, int clienteId, int produtoId, decimal valor, decimal rentabilidade, DateTime data) : base (id)
        {
            ClienteId = clienteId;
            ProdutoId = produtoId;
            Valor = valor;
            Rentabilidade = rentabilidade;
            Data = data;
        }

        public int ClienteId { get; private set; }
        public int ProdutoId { get; private set; }        
        public decimal Valor { get; private set; }
        public decimal Rentabilidade { get; private set; }
        public DateTime Data { get; private set; }

        public Cliente Cliente { get; private set; }
        public Produto Produto { get; private set; }
    }
}
