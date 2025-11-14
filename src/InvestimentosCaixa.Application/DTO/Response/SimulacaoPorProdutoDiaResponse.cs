namespace InvestimentosCaixa.Application.DTO.Response
{
    public class SimulacaoPorProdutoDiaResponse
    {
        public string Produto { get; set; }
        public DateTime Data { get; set; }
        public int QuantidadeSimulacoes { get; set; }
        public decimal MediaValorFinal { get; set; }
    }
}
