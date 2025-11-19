namespace InvestimentosCaixa.Domain.Entidades
{
    public class Produto : BaseEntity
    {
        protected Produto() { }

        public Produto(int tipoProdutoId, string nome, decimal rentabilidadeAnual, short prazoMinimoMeses)
        {
            TipoProdutoId = tipoProdutoId;
            Nome = nome;
            RentabilidadeAnual = rentabilidadeAnual;
            PrazoMinimoMeses = prazoMinimoMeses;
        }

        public Produto(int tipoProdutoId, string nome, decimal rentabilidadeAnual, short prazoMinimoMeses, TipoProduto tipoProduto)
        {
            TipoProdutoId = tipoProdutoId;
            Nome = nome;
            RentabilidadeAnual = rentabilidadeAnual;
            PrazoMinimoMeses = prazoMinimoMeses;
            TipoProduto = tipoProduto;
        }

        public int TipoProdutoId { get; private set; }
        public string Nome { get; private set; }
        public decimal RentabilidadeAnual { get; private set; }
        public short PrazoMinimoMeses { get; private set; }

        public TipoProduto TipoProduto { get; private set; }
        public ICollection<Simulacao> Simulacoes { get; private set; }

        public decimal CalcularValorFinal(decimal valor, short prazoMeses)
        {
            return valor * (1 + RentabilidadeAnual * (prazoMeses / 12m));
        }
    }
}
