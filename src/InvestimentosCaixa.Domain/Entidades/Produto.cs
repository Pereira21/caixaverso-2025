namespace InvestimentosCaixa.Domain.Entidades
{
    public class Produto : BaseEntity
    {
        protected Produto() { }

        public Produto(int tipoProdutoId, string nome, decimal rentabilidadeAnual, int prazoMinimoMeses)
        {
            TipoProdutoId = tipoProdutoId;
            Nome = nome;
            RentabilidadeAnual = rentabilidadeAnual;
            PrazoMinimoMeses = prazoMinimoMeses;
        }

        public int TipoProdutoId { get; private set; }
        public string Nome { get; private set; }
        public decimal RentabilidadeAnual { get; private set; }
        public int PrazoMinimoMeses { get; private set; }

        public TipoProduto TipoProduto { get; private set; }
        public ICollection<Simulacao> Simulacoes { get; set; }
    }
}
