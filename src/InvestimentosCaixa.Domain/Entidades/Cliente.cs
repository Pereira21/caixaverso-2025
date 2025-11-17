namespace InvestimentosCaixa.Domain.Entidades
{
    public class Cliente : BaseEntity
    {
        public Cliente() { }

        public Cliente(int id, string nome = "") : base(id)
        {
            Nome = nome;
        }

        public string Nome { get; private set; }
    }
}
