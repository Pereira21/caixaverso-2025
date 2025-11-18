namespace InvestimentosCaixa.Application.DTO.Response
{
    public class SimularInvestimentoResponse
    {
        public ProdutoValidadoResponse ProdutoValidado { get; set; }
        public ResultadoSimulacaoResponse ResultadoSimulacao { get; set; }
        public DateTime DataSimulacao { get; set; }
    }

    public class ProdutoValidadoResponse
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Tipo { get; set; }
        public decimal Rentabilidade { get; set; }
        public string Risco { get; set; }
    }

    public class ResultadoSimulacaoResponse
    {
        public decimal ValorFinal { get; set; }
        public decimal RentabilidadeEfetiva { get; set; }
        public int PrazoMeses { get; set; }
    }
}
