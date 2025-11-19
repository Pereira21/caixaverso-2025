namespace InvestimentosCaixa.Domain.Entidades
{
    public class PerfilClassificacao : BaseEntity
    {
        public PerfilClassificacao() { }

        public PerfilClassificacao(int perfilRiscoId, int minPontuacao, int maxPontuacao, PerfilRisco perfilRisco)
        {
            PerfilRiscoId = perfilRiscoId;
            MinPontuacao = minPontuacao;
            MaxPontuacao = maxPontuacao;
            PerfilRisco = perfilRisco;
        }

        public int PerfilRiscoId { get; private set; }
        public int MinPontuacao { get; private set; }
        public int MaxPontuacao { get; private set; }

        public PerfilRisco PerfilRisco { get; private set; }
    }
}
