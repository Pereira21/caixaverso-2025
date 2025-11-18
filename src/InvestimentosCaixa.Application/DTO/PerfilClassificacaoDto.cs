namespace InvestimentosCaixa.Application.DTO
{
    public class PerfilClassificacaoDto
    {
        public int Id { get; set; }
        public int PerfilRiscoId { get; set; }
        public int MinPontuacao { get; set; }
        public int MaxPontuacao { get; set; }
        public PerfilRiscoDto PerfilRisco { get; set; }
    }
}
