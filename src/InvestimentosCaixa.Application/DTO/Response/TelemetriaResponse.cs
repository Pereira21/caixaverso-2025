namespace InvestimentosCaixa.Application.DTO.Response
{
    public class TelemetriaResponse
    {
        public List<ServicoResponse> Servicos { get; set; }
        public PeriodoResponse Periodo { get; set; }
    }

    public class ServicoResponse
    {
        public string Nome { get; set; }
        public int QuantidadeChamadas { get; set; }
        public double MediaTempoRespostaMs { get; set; }
    }

    public class PeriodoResponse
    {
        public DateOnly Inicio { get; set; }
        public DateOnly Fim { get; set; }
    }
}
