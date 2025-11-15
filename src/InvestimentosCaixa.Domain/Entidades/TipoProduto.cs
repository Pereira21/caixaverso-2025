namespace InvestimentosCaixa.Domain.Entidades
{
    public class TipoProduto : BaseEntity
    {
        protected TipoProduto()
        {
            Produtos = new List<Produto>();
        }

        public TipoProduto(string nome, int riscoId, string liquidez, string descricao)
        {
            Nome = nome;
            RiscoId = riscoId;
            Liquidez = liquidez;
            Descricao = descricao;

            Produtos = new List<Produto>();
        }

        public string Nome { get; private set; }
        public int RiscoId { get; private set; }
        public string Liquidez { get; private set; }
        public string Descricao { get; private set; }

        public Risco Risco { get; set; }
        public ICollection<Produto> Produtos { get; private set; }
    }
}
