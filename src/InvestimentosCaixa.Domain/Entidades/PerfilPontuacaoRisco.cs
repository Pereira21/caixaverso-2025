namespace InvestimentosCaixa.Domain.Entidades
{
    public class PerfilPontuacaoRisco : BaseEntity
    {
        public int RiscoId { get; private set; }
        public int PontosBase { get; private set; }
        public decimal Multiplicador { get; private set; }
        public int PontosMaximos { get; private set; }

        public Risco Risco { get; set; }
    }
}
