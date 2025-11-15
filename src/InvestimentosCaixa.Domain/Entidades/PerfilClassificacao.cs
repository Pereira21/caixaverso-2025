namespace InvestimentosCaixa.Domain.Entidades
{
    public class PerfilClassificacao : BaseEntity
    {
        public int PerfilRiscoId { get; set; }
        public int MinPontuacao { get; private set; }
        public int MaxPontuacao { get; private set; }

        public PerfilRisco PerfilRisco { get; set; }
    }
}
