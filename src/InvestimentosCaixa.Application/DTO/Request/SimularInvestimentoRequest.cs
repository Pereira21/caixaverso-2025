namespace InvestimentosCaixa.Application.DTO.Request
{
    public class SimularInvestimentoRequest
    {
        public int ClienteId { get; set; }
        public decimal Valor { get; set; }
        public short PrazoMeses { get; set; }
        public string TipoProduto { get; set; }
    }
}
