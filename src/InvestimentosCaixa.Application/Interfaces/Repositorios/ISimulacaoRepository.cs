using InvestimentosCaixa.Application.DTO.Response;
using InvestimentosCaixa.Domain.Entidades;

namespace InvestimentosCaixa.Application.Interfaces.Repositorios
{
    public interface ISimulacaoRepository : IRepository<Simulacao>
    {
        Task<List<Simulacao>> ObterTodosComProdutoAsync(int pagina, int tamanhoPagina);
        Task<List<Simulacao>> ObterComProdutoPorClienteId(int clienteId);

        Task<List<SimulacaoPorProdutoDiaResponse>> ObterSimulacoesPorProdutoDiaAsync();
    }
}
