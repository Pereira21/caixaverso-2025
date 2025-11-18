
using InvestimentosCaixa.Application.DTO;
using InvestimentosCaixa.Domain.Entidades;

namespace InvestimentosCaixa.Application.Interfaces.Repositorios
{
    public interface IPerfilRiscoRepository
    {
        Task<PerfilRisco?> ObterComRiscoPorNome(string nome);

        Task<PerfilPontuacaoVolumeDto?> ObterPerfilPontuacaoVolume(decimal volumeInvestido);
        Task<PerfilPontuacaoFrequenciaDto?> ObterPerfilPontuacaoFrequencia(int totalSimulacoes);
        Task<List<PerfilPontuacaoRiscoDto>> ObterPerfilPontuacaoRiscoPorRiscos(List<int> riscoIdList);
        Task<PerfilClassificacaoDto?> ObterPerfilClassificacaoPorPontuacao(int pontuacaoCliente);        
    }
}
