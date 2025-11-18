namespace InvestimentosCaixa.Domain.Entidades
{
    public class PerfilClassificacao : BaseEntity
    {
        public PerfilClassificacao() { }

        public int PerfilRiscoId { get; private set; }
        public int MinPontuacao { get; private set; }
        public int MaxPontuacao { get; private set; }

        public PerfilRisco PerfilRisco { get; private set; }
    }
}
