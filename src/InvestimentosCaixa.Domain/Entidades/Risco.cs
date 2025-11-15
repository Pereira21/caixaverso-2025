namespace InvestimentosCaixa.Domain.Entidades
{
    public class Risco : BaseEntity
    {
        public Risco() { }

        public Risco(string nome, string descricao)
        {
            Nome = nome;
            Descricao = descricao;
        }

        public string Nome { get; private set; }
        public string Descricao { get; private set; }
    }
}
