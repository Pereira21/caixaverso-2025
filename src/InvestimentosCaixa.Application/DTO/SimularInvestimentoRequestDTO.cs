namespace InvestimentosCaixa.Application.DTO
{
    public class SimularInvestimentoRequestDTO
    {
        public int ClienteId { get; set; }
        public decimal Valor { get; set; }
        public short PrazoMeses { get; set; }
        public string TipoProduto { get; set; }
    }
}
