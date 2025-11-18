namespace InvestimentosCaixa.Domain.Entidades
{
    public class RelPerfilRisco : BaseEntity
    {
        public RelPerfilRisco() { }

        public int PerfilRiscoId { get; private set; }
        public int RiscoId { get; private set; }


        public Risco Risco { get; private set; }
        public PerfilRisco PerfilRisco { get; private set; }
    }
}
