namespace InvestimentosCaixa.Application.DTO.Response
{
    public class InvestimentoResponse
    {
        public int Id { get; set; }
        public string Tipo { get; set; }
        public decimal Valor { get; set; }
        public decimal Rentabilidade { get; set; }
        public string Data { get; set; }
    }
}