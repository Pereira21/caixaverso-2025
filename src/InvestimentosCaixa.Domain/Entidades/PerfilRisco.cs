namespace InvestimentosCaixa.Domain.Entidades
{
    public class PerfilRisco : BaseEntity
    {
        public PerfilRisco() { }

        public string Nome { get; private set; }
        public string Descricao { get; private set; }

        public List<RelPerfilRisco> RelPerfilRiscoList { get; private set; }
    }
}
