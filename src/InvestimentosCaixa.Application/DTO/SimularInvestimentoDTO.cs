namespace InvestimentosCaixa.Application.DTO
{
    public class SimularInvestimentoDTO
    {
        public Produto ProdutoValidado { get; set; }
        public Simulacao ResultadoSimulacao { get; set; }
        public DateTime DataSimulacao { get; set; }
    }

    public class Produto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Tipo { get; set; }
        public decimal Rentabilidade { get; set; }
        public string Risco { get; set; }
    }

    public class Simulacao
    {
        public decimal ValorFinal { get; set; }
        public decimal RentabilidadeEfetiva { get; set; }
        public int PrazoMeses { get; set; }
    }
}
