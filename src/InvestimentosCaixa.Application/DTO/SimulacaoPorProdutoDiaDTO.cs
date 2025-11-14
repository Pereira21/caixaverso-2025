namespace InvestimentosCaixa.Application.DTO
{
    public class SimulacaoPorProdutoDiaDTO
    {
        public string Produto { get; set; }
        public DateTime Data { get; set; }
        public int QuantidadeSimulacoes { get; set; }
        public decimal MediaValorFinal { get; set; }
    }
}
