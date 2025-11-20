using AutoMapper;
using InvestimentosCaixa.Application.DTO.Response;
using InvestimentosCaixa.Application.Interfaces.Repositorios;
using InvestimentosCaixa.Application.Interfaces.Services;
using InvestimentosCaixa.Application.Notificacoes;
using InvestimentosCaixa.Domain.Entidades;
using Microsoft.Extensions.Logging;

namespace InvestimentosCaixa.Application.Services
{
    public class InvestimentoService : BaseService, IInvestimentoService
    {
        private readonly IInvestimentoRepository _investimentoRepository;
        private readonly ILogger<InvestimentoService> _logger;

        public InvestimentoService(INotificador notificador, IMapper mapper, IUnitOfWork unitOfWork, IInvestimentoRepository investimentoRepository, ILogger<InvestimentoService> logger) : base(notificador, mapper, unitOfWork)
        {
            _logger = logger;
            _investimentoRepository = investimentoRepository;
        }

        public async Task<List<InvestimentoResponse>> ObterPorClienteIdAsync(Guid userId, string userEmail, int clienteId, int pagina, int tamanhoPagina)
        {
            _logger.LogInformation($"O analista {userId} - Email: {userEmail} está obtendo todos os investimentos do cliente {clienteId}!");

            List<Investimento> investimentos = await _investimentoRepository.ObterPaginadoComProdutoPorClienteIdAsync(clienteId, pagina, tamanhoPagina);
            return _mapper.Map<List<InvestimentoResponse>>(investimentos);
        }
    }
}
