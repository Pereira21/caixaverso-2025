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

        public PerfilRiscoService(INotificador notificador, IMapper mapper, IUnitOfWork unitOfWork, ISimulacaoRepository simulacaoRepository) : 
            base(notificador, mapper, unitOfWork)
        {
            _simulacaoRepository = simulacaoRepository;
        }

        public async Task<PerfilRiscoResponse> ObterPorClienteId(int clienteId)
        {
            var simulacoesPorCliente = await _simulacaoRepository.ObterPorClienteId(clienteId);

            if(simulacoesPorCliente == null || !simulacoesPorCliente.Any())
            {
                Notificar("Cliente não possui simulações para determinar um Perfil de Risco!");
                return null;
            }

            return new PerfilRiscoResponse
            {
                ClienteId = clienteId,
                Perfil = null,
                Pontuacao = 0,
                Descricao = ""
            };
        }
    }
}
