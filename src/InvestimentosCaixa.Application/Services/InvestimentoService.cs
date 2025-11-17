using AutoMapper;
using InvestimentosCaixa.Application.DTO.Response;
using InvestimentosCaixa.Application.Interfaces.Repositorios;
using InvestimentosCaixa.Application.Interfaces.Services;
using InvestimentosCaixa.Application.Notificacoes;
using InvestimentosCaixa.Domain.Entidades;

namespace InvestimentosCaixa.Application.Services
{
    public class InvestimentoService : BaseService, IInvestimentoService
    {
        private readonly IInvestimentoRepository _investimentoRepository;

        public InvestimentoService(INotificador notificador, IMapper mapper, IUnitOfWork unitOfWork, IInvestimentoRepository investimentoRepository) : base(notificador, mapper, unitOfWork)
        {
            _investimentoRepository = investimentoRepository;
        }

        public async Task<List<InvestimentoResponse>> ObterPorClienteId(int clienteId)
        {
            List<Investimento> investimentos = await _investimentoRepository.ObterComProdutoPorClienteId(clienteId);
            return _mapper.Map<List<InvestimentoResponse>>(investimentos);
        }
    }
}
