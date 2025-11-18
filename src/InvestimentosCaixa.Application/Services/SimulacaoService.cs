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
            : base(notificador, mapper, unitOfWork)
        {
            _produtoRepository = produtoRepository;
            _simulacaoRepository = simulacaoRepository;
            _clienteRepository = clienteRepository;
        }

        public async Task<SimularInvestimentoResponse?> SimularInvestimento(SimularInvestimentoRequest request)
        {
            var dataSimulacao = DateTime.UtcNow;
            var produtoBaseDto = await _produtoRepository.ObterAdequadoAsync(request.PrazoMeses, request.TipoProduto);
            var cliente = await _clienteRepository.ObterPeloIdAsync(request.ClienteId);

            if (produtoBaseDto == null)
            {
                Notificar("Nenhum produto encontrado para essa simulação!");
                return null;
            }

            var produtoAdequado = _mapper.Map<Produto>(produtoBaseDto);
            var valorFinal = produtoAdequado.CalcularValorFinal(request.Valor, request.PrazoMeses);

            var response = new SimularInvestimentoResponse
            {
                ProdutoValidado = MapearProdutoValidadoResponse(produtoAdequado),
                ResultadoSimulacao = MapearResultadoSimulacaoResponse(valorFinal, produtoAdequado.RentabilidadeAnual, request.PrazoMeses),
                DataSimulacao = dataSimulacao
            };

            if (cliente == null)
                await _clienteRepository.AdicionarAsync(new Cliente(request.ClienteId));

            await _simulacaoRepository.AdicionarAsync(new Simulacao(request.ClienteId, produtoAdequado.Id, request.Valor, valorFinal, request.PrazoMeses, produtoAdequado.RentabilidadeAnual, dataSimulacao));
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

        #region metodos privados
        private ResultadoSimulacaoResponse MapearResultadoSimulacaoResponse(decimal valorFinal, decimal rentabilidadeAnual, short prazoMeses)
        {
            return new ResultadoSimulacaoResponse
            {
                ValorFinal = decimal.Round(valorFinal, 2),
                RentabilidadeEfetiva = rentabilidadeAnual,
                PrazoMeses = prazoMeses
            };
        }

        private ProdutoValidadoResponse MapearProdutoValidadoResponse(Produto produtoAdequado)
        {
            return new ProdutoValidadoResponse
            {
                Id = produtoAdequado.Id,
                Nome = produtoAdequado.Nome,
                Tipo = produtoAdequado.TipoProduto != null ? produtoAdequado.TipoProduto.Nome : string.Empty,
                Rentabilidade = produtoAdequado.RentabilidadeAnual,
                Risco = produtoAdequado.TipoProduto != null ? produtoAdequado.TipoProduto.Risco != null ? produtoAdequado.TipoProduto.Risco.Nome : string.Empty : string.Empty,
            };
        }
        #endregion
    }
}
