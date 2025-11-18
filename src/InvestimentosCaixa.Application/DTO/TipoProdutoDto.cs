namespace InvestimentosCaixa.Application.DTO
{
    public class TipoProdutoDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int RiscoId { get; set; }
        public string Liquidez { get; set; }
        public string Descricao { get; set; }
        public RiscoDto Risco { get; set; }
    }
}
