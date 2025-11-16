namespace InvestimentosCaixa.Domain.Entidades
{
    public class RelPerfilRisco : BaseEntity
    {
        public int PerfilRiscoId { get; set; }
        public int RiscoId { get; set; }


        public Risco Risco { get; set; }
        public PerfilRisco PerfilRisco { get; set; }
    }
}
