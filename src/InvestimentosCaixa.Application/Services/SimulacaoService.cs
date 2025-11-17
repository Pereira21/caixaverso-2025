using AutoMapper;
using InvestimentosCaixa.Application.DTO.Request;
using InvestimentosCaixa.Application.DTO.Response;
using InvestimentosCaixa.Application.Interfaces.Repositorios;
using InvestimentosCaixa.Application.Interfaces.Services;
using InvestimentosCaixa.Application.Notificacoes;
using InvestimentosCaixa.Domain.Entidades;

namespace InvestimentosCaixa.Application.Services
{
    public class SimulacaoService : BaseService, ISimulacaoService
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly ISimulacaoRepository _simulacaoRepository;
        private readonly IClienteRepository _clienteRepository;

        public SimulacaoService(INotificador notificador, IMapper mapper, IUnitOfWork unitOfWork, IProdutoRepository produtoRepository, ISimulacaoRepository simulacaoRepository, IClienteRepository clienteRepository) 
            : base (notificador, mapper, unitOfWork)
        {
            _produtoRepository = produtoRepository;
            _simulacaoRepository = simulacaoRepository;            
            _clienteRepository = clienteRepository;
        }

        public async Task<SimularInvestimentoResponse> SimularInvestimento(SimularInvestimentoRequest request)
        {
            var produtoAdequado = await _produtoRepository.ObterAdequadoAsync(request.PrazoMeses, request.TipoProduto);
            var cliente = await _clienteRepository.ObterPeloIdAsync(request.ClienteId);

            if (produtoAdequado == null)
                Notificar("Nenhum produto encontrado para essa simulação!");

            //Se houver outras validações, concentro o retorno somente nesse ponto
            if (_notificador.TemNotificacao())
                return null;

            var valorFinal = request.Valor * (1 + produtoAdequado.RentabilidadeAnual * (produtoAdequado.PrazoMinimoMeses / 12m));

            var response = new SimularInvestimentoResponse
            {
                ProdutoValidado = new ProdutoDTO { Id = produtoAdequado.Id, Nome = produtoAdequado.Nome, Tipo = produtoAdequado.TipoProduto.Nome, Rentabilidade = produtoAdequado.RentabilidadeAnual, Risco = produtoAdequado.TipoProduto.Risco.Nome },
                ResultadoSimulacao = new SimulacaoDTO { ValorFinal = decimal.Round(valorFinal, 2), RentabilidadeEfetiva = produtoAdequado.RentabilidadeAnual, PrazoMeses = produtoAdequado.PrazoMinimoMeses },
                DataSimulacao = DateTime.UtcNow
            };

            if (cliente == null)
                await _clienteRepository.AdicionarAsync(new Cliente(request.ClienteId));

            await _simulacaoRepository.AdicionarAsync(new Simulacao(request.ClienteId, produtoAdequado.Id, request.Valor, valorFinal, request.PrazoMeses, produtoAdequado.RentabilidadeAnual, DateTime.UtcNow));
            await _unitOfWork.SaveChangesAsync();

            return response;
        }

        public async Task<List<SimulacaoResponseDTO>> ObterHistorico()
        {
            var simulacoes = await _simulacaoRepository.ObterTodosComProdutoAsync();

            return _mapper.Map<List<SimulacaoResponseDTO>>(simulacoes);
        }

        public async Task<List<SimulacaoPorProdutoDiaResponse>> ObterPorProdutoDiaAsync()
        {
            return await _simulacaoRepository.ObterSimulacoesPorProdutoDiaAsync();
        }
    }
}
