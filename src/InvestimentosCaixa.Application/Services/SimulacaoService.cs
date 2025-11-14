using InvestimentosCaixa.Application.DTO;
using InvestimentosCaixa.Application.Interfaces;
using InvestimentosCaixa.Application.Notificacoes;
using InvestimentosCaixa.Domain.Entidades;
using InvestimentosCaixa.Domain.Repositorios;
using System.Threading;

namespace InvestimentosCaixa.Application.Services
{
    public class SimulacaoService : BaseService, ISimulacaoService
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly ISimulacaoRepository _simulacaoRepository;
        private readonly IUnitOfWork _uow;

        public SimulacaoService(INotificador notificador, IUnitOfWork uow, IProdutoRepository produtoRepository, ISimulacaoRepository simulacaoRepository) : base (notificador)
        {
            _produtoRepository = produtoRepository;
            _simulacaoRepository = simulacaoRepository;
            _uow = uow;
        }

        public async Task<SimularInvestimentoResponseDTO> SimularInvestimento(SimularInvestimentoRequestDTO request)
        {
            var produtoAdequado = await _produtoRepository.ObterAdequadoAsync(request.PrazoMeses, request.TipoProduto);

            if (produtoAdequado == null)
                Notificar("Nenhum produto encontrado para essa simulação!");

            //Se houver outras validações, concentro o retorno somente nesse ponto
            if (_notificador.TemNotificacao())
                return null;

            // 3) cálculo simples exemplo (pro rata anual)
            var valorFinal = request.Valor * (1 + produtoAdequado.RentabilidadeAnual * (produtoAdequado.PrazoMinimoMeses / 12m));

            var response = new SimularInvestimentoResponseDTO
            {
                ProdutoValidado = new ProdutoDTO { Id = produtoAdequado.Id, Nome = produtoAdequado.Nome, Tipo = produtoAdequado.TipoProduto.Nome, Rentabilidade = produtoAdequado.RentabilidadeAnual, Risco = produtoAdequado.TipoProduto.Risco },
                ResultadoSimulacao = new SimulacaoDTO { ValorFinal = decimal.Round(valorFinal, 2), RentabilidadeEfetiva = produtoAdequado.RentabilidadeAnual, PrazoMeses = produtoAdequado.PrazoMinimoMeses },
                DataSimulacao = DateTime.UtcNow
            };

            // 4) persistir simulação (se tiver repositório Simulacao, adiciona aqui e salva)
            await _simulacaoRepository.AdicionarAsync(new Simulacao(request.ClienteId, produtoAdequado.Id, request.Valor, valorFinal, request.PrazoMeses, produtoAdequado.RentabilidadeAnual, DateTime.UtcNow));
            await _uow.SaveChangesAsync();

            return response;
        }

        public async Task<List<SimulacaoResponseDTO>> ObterHistorico()
        {
            var simulacoes = await _simulacaoRepository.ObterTodosComProdutoAsync();

            return simulacoes.Select(s => new SimulacaoResponseDTO
            {
                Id = s.Id,
                ClienteId = s.ClienteId,
                Produto = s.Produto?.Nome ?? string.Empty,
                ValorInvestido = s.ValorInvestido,
                ValorFinal = s.ValorFinal,
                PrazoMeses = s.PrazoMeses,
                DataSimulacao = s.DataSimulacao
            }
            ).ToList();
        }
    }
}
