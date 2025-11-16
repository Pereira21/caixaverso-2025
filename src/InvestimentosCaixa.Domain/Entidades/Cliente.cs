namespace InvestimentosCaixa.Domain.Entidades
{
    public class Cliente : BaseEntity
    {
        public Cliente() { }

        public Cliente(string nome)
        {
            Nome = nome;
        }

        public string Nome { get; private set; }
    }
}
