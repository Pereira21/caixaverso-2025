namespace InvestimentosCaixa.Domain.Entidades
{
    public class TipoProduto
    {
        protected TipoProduto()
        {
            Produtos = new List<Produto>();
        }

        public TipoProduto(string nome, string risco, string liquidez, string descricao)
        {
            Nome = nome;
            Risco = risco;
            Liquidez = liquidez;
            Descricao = descricao;

            Produtos = new List<Produto>();
        }

        public int Id { get; private set; }

        public string Nome { get; private set; }
        public string Risco { get; private set; }
        public string Liquidez { get; private set; }
        public string Descricao { get; private set; }

        public ICollection<Produto> Produtos { get; private set; }
    }
}
