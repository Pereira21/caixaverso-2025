using AutoMapper;
using InvestimentosCaixa.Application.DTO.Response;
using InvestimentosCaixa.Application.Interfaces.Repositorios;
using InvestimentosCaixa.Application.Interfaces.Services;
using InvestimentosCaixa.Application.Notificacoes;

namespace InvestimentosCaixa.Application.Services
{
    public class PerfilRiscoService : BaseService, IPerfilRiscoService
    {
        private readonly ISimulacaoRepository _simulacaoRepository;
        private readonly IPerfilRiscoRepository _perfilRiscoRepository;

        public PerfilRiscoService(INotificador notificador, IMapper mapper, IUnitOfWork unitOfWork, ISimulacaoRepository simulacaoRepository, IPerfilRiscoRepository perfilRiscoRepository) : 
            base(notificador, mapper, unitOfWork)
        {
            _simulacaoRepository = simulacaoRepository;
            _perfilRiscoRepository = perfilRiscoRepository;
        }

        public async Task<PerfilRiscoResponse> ObterPorClienteId(int clienteId)
        {
            int pontuacaoCliente = 0;
            var simulacoesPorCliente = await _simulacaoRepository.ObterPorClienteId(clienteId);

            if(simulacoesPorCliente == null || !simulacoesPorCliente.Any())
            {
                Notificar("Cliente não possui simulações para determinar um Perfil de Risco!");
                return null;
            }

            var perfilPontuacaoVolume = await _perfilRiscoRepository.ObterPerfilPontuacaoVolume(simulacoesPorCliente.Sum(x => x.ValorInvestido));

            if (perfilPontuacaoVolume != null)
            {
                pontuacaoCliente += perfilPontuacaoVolume.Pontos;
            }
            // falta risco, frequencia

            return new PerfilRiscoResponse
            {
                ClienteId = clienteId,
                Pontuacao = pontuacaoCliente,
                Perfil = null,
                Descricao = ""
            };
        }
    }
}
