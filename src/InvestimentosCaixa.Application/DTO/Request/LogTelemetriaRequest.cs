namespace InvestimentosCaixa.Application.DTO.Request
{
    public class LogTelemetriaRequest
    {
        public string Endpoint { get; set; }
        public string Metodo { get; set; }
        public int TempoRespostaMs { get; set; }
        public bool Sucesso { get; set; }
        public DateTime DataRegistro { get; set; }
    }
}
