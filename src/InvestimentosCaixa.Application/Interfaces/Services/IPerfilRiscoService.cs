using InvestimentosCaixa.Application.DTO.Response;

namespace InvestimentosCaixa.Application.Interfaces.Services
{
    public interface IPerfilRiscoService
    {
        Task<PerfilRiscoResponse?> ObterPorClienteId(int clienteId);

        Task<IEnumerable<ProdutoRecomendadoResponse>> ObterProdutosRecomendadosPorPerfil(string perfil);
    }
}
