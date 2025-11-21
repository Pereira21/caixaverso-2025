
using InvestimentosCaixa.Application.DTO;
using InvestimentosCaixa.Domain.Entidades;

namespace InvestimentosCaixa.Application.Interfaces.Repositorios
{
    public interface IPerfilRiscoRepository : IRepository<PerfilRisco>
    {
        /// <summary>
        /// Obter perfil de risco por nome com os riscos associados
        /// </summary>
        /// <param name="nome"></param>
        /// <returns></returns>
        Task<PerfilRisco?> ObterComRiscoPorNomeAsync(string nome);

        /// <summary>
        /// Obter parâmetros de pontuação por volume investido ou simulado
        /// </summary>
        /// <param name="volumeInvestido"></param>
        /// <returns></returns>
        Task<PerfilPontuacaoVolumeDto?> ObterPerfilPontuacaoVolumeAsync(decimal volumeInvestido);

        /// <summary>
        /// Obter parâmetros de pontuação por frequência de movimentações ou simulações
        /// </summary>
        /// <param name="totalSimulacoes"></param>
        /// <returns></returns>
        Task<PerfilPontuacaoFrequenciaDto?> ObterPerfilPontuacaoFrequenciaAsync(int totalSimulacoes);

        /// <summary>
        /// Obter parâmetros de pontuação por riscos associados a produtos investidos ou simulados
        /// </summary>
        /// <param name="riscoIdList"></param>
        /// <returns></returns>
        Task<List<PerfilPontuacaoRiscoDto>> ObterPerfilPontuacaoRiscoPorRiscosAsync(List<int> riscoIdList);

        /// <summary>
        /// Obter perfil de classificação por pontuação total do cliente
        /// </summary>
        /// <param name="pontuacaoCliente"></param>
        /// <returns></returns>
        Task<PerfilClassificacaoDto?> ObterPerfilClassificacaoPorPontuacaoAsync(int pontuacaoCliente);        
    }
}
