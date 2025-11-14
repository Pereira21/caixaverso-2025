namespace InvestimentosCaixa.Domain.Entidades
{
    public class Simulacao : BaseEntity
    {
        protected Simulacao() { }

        public Simulacao(int clienteId, int produtoId, decimal valorInvestido, decimal valorFinal, int prazoMeses, decimal rentabilidadeEfetiva, DateTime dataSimulacao)
        {
            ClienteId = clienteId;
            ProdutoId = produtoId;
            ValorInvestido = valorInvestido;
            ValorFinal = valorFinal;
            PrazoMeses = prazoMeses;
            RentabilidadeEfetiva = rentabilidadeEfetiva;
            DataSimulacao = dataSimulacao;
        }

        public int ClienteId { get; private set; }
        public int ProdutoId { get; private set; }
        public decimal ValorInvestido { get; private set; }
        public decimal ValorFinal { get; private set; }
        public int PrazoMeses { get; private set; }
        public decimal RentabilidadeEfetiva { get; private set; }
        public DateTime DataSimulacao { get; private set; }

        public Produto Produto { get; private set; }
    }
}
