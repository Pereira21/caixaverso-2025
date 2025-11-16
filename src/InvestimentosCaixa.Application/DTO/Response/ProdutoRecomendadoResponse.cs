namespace InvestimentosCaixa.Application.DTO.Response
{
    public class ProdutoRecomendadoResponse
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Tipo { get; set; }
        public decimal Rentabilidade { get; set; }
        public string Risco { get; set; }
    }
} 