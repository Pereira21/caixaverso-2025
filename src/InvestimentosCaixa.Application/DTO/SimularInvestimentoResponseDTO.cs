namespace InvestimentosCaixa.Application.DTO
{
    public class SimularInvestimentoResponseDTO
    {
        public ProdutoDTO ProdutoValidado { get; set; }
        public SimulacaoDTO ResultadoSimulacao { get; set; }
        public DateTime DataSimulacao { get; set; }
    }

    public class ProdutoDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Tipo { get; set; }
        public decimal Rentabilidade { get; set; }
        public string Risco { get; set; }
    }

    public class SimulacaoDTO
    {
        public decimal ValorFinal { get; set; }
        public decimal RentabilidadeEfetiva { get; set; }
        public int PrazoMeses { get; set; }
    }
}
