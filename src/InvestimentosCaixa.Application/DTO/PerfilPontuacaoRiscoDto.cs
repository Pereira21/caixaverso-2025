namespace InvestimentosCaixa.Application.DTO
{
    public class PerfilPontuacaoRiscoDto
    {
        public int Id { get; set; }
        public int RiscoId { get; set; }
        public int PontosBase { get; set; }
        public decimal Multiplicador { get; set; }
        public int PontosMaximos { get; set; }
    }
}
