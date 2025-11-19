using InvestimentosCaixa.Application.DTO.Response;

namespace InvestimentosCaixa.Api.Models.Produto
{
    public class TipoProdutoDisponivelModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string RiscoNome { get; set; }
        public string Liquidez { get; set; }
        public List<ProdutoDisponivelModel> Produtos { get; set; }
    }
}
