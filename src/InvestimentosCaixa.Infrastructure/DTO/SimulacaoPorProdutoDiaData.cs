namespace InvestimentosCaixa.Infrastructure.DTO
{
    public class SimulacaoPorProdutoDiaData
    {
        public string Produto { get; set; }
        public DateTime Data { get; set; }
        public int QuantidadeSimulacoes { get; set; }
        public decimal MediaValorFinal { get; set; }
    }
}
