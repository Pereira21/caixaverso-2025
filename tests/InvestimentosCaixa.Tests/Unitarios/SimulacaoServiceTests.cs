using AutoMapper;
using InvestimentosCaixa.Application.DTO;
using InvestimentosCaixa.Application.DTO.Request;
using InvestimentosCaixa.Application.DTO.Response;
using InvestimentosCaixa.Application.Interfaces.Repositorios;
using InvestimentosCaixa.Application.Notificacoes;
using InvestimentosCaixa.Application.Resources;
using InvestimentosCaixa.Application.Services;
using InvestimentosCaixa.Domain.Entidades;
using Microsoft.Extensions.Logging;
using Moq;

namespace InvestimentosCaixa.Tests.Unitarios
{
    public class SimulacaoServiceTests
    {
        private readonly Mock<ILogger<SimulacaoService>> _loggerMock;
        private readonly Mock<INotificador> _notificadorMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly Mock<IProdutoRepository> _produtoRepoMock;
        private readonly Mock<ISimulacaoRepository> _simulacaoRepoMock;
        private readonly Mock<IClienteRepository> _clienteRepoMock;

        private readonly SimulacaoService _service;

        public SimulacaoServiceTests()
        {
            _loggerMock = new Mock<ILogger<SimulacaoService>>();
            _notificadorMock = new Mock<INotificador>();
            _mapperMock = new Mock<IMapper>();
            _uowMock = new Mock<IUnitOfWork>();
            _produtoRepoMock = new Mock<IProdutoRepository>();
            _simulacaoRepoMock = new Mock<ISimulacaoRepository>();
            _clienteRepoMock = new Mock<IClienteRepository>();

            _service = new SimulacaoService(
                _loggerMock.Object,
                _notificadorMock.Object,
                _mapperMock.Object,
                _uowMock.Object,
                _produtoRepoMock.Object,
                _simulacaoRepoMock.Object,
                _clienteRepoMock.Object
            );
        }

        [Fact(DisplayName = "Se produto não encontrado deve retornar null e notificar")]
        public async Task SimularInvestimento_ProdutoNaoEncontrado_DeveRetornarNullENotificar()
        {
            // Arrange
            var req = new SimularInvestimentoRequest
            {
                ClienteId = 10,
                PrazoMeses = 12,
                TipoProduto = "Criptomoedas",
                Valor = 1000
            };

            _produtoRepoMock.Setup(x => x.ObterAdequadoAsync(req.PrazoMeses, req.TipoProduto))
                            .ReturnsAsync((ProdutoDto?)null);

            // Act
            var result = await _service.SimularInvestimento(req);

            // Assert
            Assert.Null(result);

            _notificadorMock.Verify(n => n.Handle(It.Is<Notificacao>(x => x.Mensagem == Mensagens.NenhumProdutoEncontradoSimulacao)),
                Times.Once
            );
        }

        [Fact(DisplayName = "Se cliente não existe deve adicionar cliente")]
        public async Task SimularInvestimento_ClienteNaoExiste_DeveAdicionarCliente()
        {
            // Arrange
            var req = new SimularInvestimentoRequest
            {
                ClienteId = 1,
                PrazoMeses = 12,
                TipoProduto = "CDB",
                Valor = 1000
            };

            var produtoBaseDto = new ProdutoDto
            {
                Id = 10,
                Nome = "CDB Teste",
                RentabilidadeAnual = 0.12m
            };

            var produtoMapa = new Produto(2, "CDB Teste", 0.12m, 6);

            _produtoRepoMock.Setup(x => x.ObterAdequadoAsync(req.PrazoMeses, req.TipoProduto))
                            .ReturnsAsync(produtoBaseDto);

            _mapperMock.Setup(m => m.Map<Produto>(It.IsAny<ProdutoDto>()))
                       .Returns(produtoMapa);

            _clienteRepoMock.Setup(x => x.ObterPeloIdAsync(req.ClienteId))
                            .ReturnsAsync((Cliente?)null);

            // Act
            var result = await _service.SimularInvestimento(req);

            // Assert
            _clienteRepoMock.Verify(x => x.AdicionarAsync(It.IsAny<Cliente>()), Times.Once);
            Assert.NotNull(result);
        }

        [Fact(DisplayName = "Se cliente existe não deve adicionar novo cliente")]
        public async Task SimularInvestimento_ClienteExiste_NaoDeveAdicionarCliente()
        {
            // Arrange
            var req = new SimularInvestimentoRequest
            {
                ClienteId = 1,
                PrazoMeses = 12,
                TipoProduto = "CDB",
                Valor = 500
            };

            var produtoBaseDto = new ProdutoDto
            {
                Id = 10,
                Nome = "CDB Teste",
                RentabilidadeAnual = 0.12m
            };

            var produtoMapa = new Produto(2, "CDB Teste", 0.12m, 12);

            _produtoRepoMock.Setup(x => x.ObterAdequadoAsync(req.PrazoMeses, req.TipoProduto))
                            .ReturnsAsync(produtoBaseDto);

            _mapperMock.Setup(m => m.Map<Produto>(It.IsAny<ProdutoDto>()))
                       .Returns(produtoMapa);

            _clienteRepoMock.Setup(x => x.ObterPeloIdAsync(req.ClienteId))
                            .ReturnsAsync(new Cliente(req.ClienteId));

            // Act
            var result = await _service.SimularInvestimento(req);

            // Assert
            _clienteRepoMock.Verify(x => x.AdicionarAsync(It.IsAny<Cliente>()), Times.Never);
            Assert.NotNull(result);
        }

        [Fact(DisplayName = "Deve adicionar simulação e salvar")]
        public async Task SimularInvestimento_DeveAdicionarSimulacaoESalvar()
        {
            var req = new SimularInvestimentoRequest
            {
                ClienteId = 1,
                PrazoMeses = 12,
                TipoProduto = "CDB",
                Valor = 500
            };

            var produtoBaseDto = new ProdutoDto
            {
                Id = 10,
                Nome = "CDB Teste",
                RentabilidadeAnual = 0.12m
            };

            var produtoMapa = new Produto(2, "CDB Teste", 0.12m, 12);

            _produtoRepoMock.Setup(x => x.ObterAdequadoAsync(req.PrazoMeses, req.TipoProduto))
                            .ReturnsAsync(produtoBaseDto);

            _mapperMock.Setup(m => m.Map<Produto>(It.IsAny<ProdutoDto>()))
                       .Returns(produtoMapa);

            _clienteRepoMock.Setup(x => x.ObterPeloIdAsync(req.ClienteId))
                            .ReturnsAsync(new Cliente(req.ClienteId));

            // Act
            await _service.SimularInvestimento(req);

            // Assert
            _simulacaoRepoMock.Verify(x => x.AdicionarAsync(It.IsAny<Simulacao>()), Times.Once);
            _uowMock.Verify(x => x.SaveChangesAsync(new CancellationToken()), Times.Once);
        }

        [Fact(DisplayName = "Deve retornar lista mapeada")]
        public async Task ObterHistorico_DeveRetornarListaMapeada()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var email = "analista@teste.com";

            var simulacoes = new List<Simulacao>
            {
                new Simulacao(1, 2, 1350.10m, 1400, 12, 0.975m, DateTime.UtcNow),
                new Simulacao(1, 1, 750, 790, 6, 0.875m, DateTime.UtcNow)
            };

            var simulacoesDTO = new List<SimulacaoResponseDTO>
            {
                new SimulacaoResponseDTO { Id = 1 },
                new SimulacaoResponseDTO { Id = 2 }
            };

            _simulacaoRepoMock
                .Setup(x => x.ObterTodosPaginadoComProdutoAsync(1, 10))
                .ReturnsAsync(simulacoes);

            _mapperMock
                .Setup(x => x.Map<List<SimulacaoResponseDTO>>(simulacoes))
                .Returns(simulacoesDTO);

            // Act
            var result = await _service.ObterHistorico(userId, email, 1, 10);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(simulacoesDTO, result);

            _simulacaoRepoMock.Verify(
                x => x.ObterTodosPaginadoComProdutoAsync(1, 10),
                Times.Once);

            _mapperMock.Verify(
                x => x.Map<List<SimulacaoResponseDTO>>(simulacoes),
                Times.Once);
        }

        [Fact(DisplayName = "Deve retornar lista por produto/dia")]
        public async Task ObterPorProdutoDiaAsync_DeveRetornarLista()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var email = "analista@teste.com";

            var listaEsperada = new List<SimulacaoPorProdutoDiaResponse>
            {
                new SimulacaoPorProdutoDiaResponse
                {
                    Produto = "CDB",
                    Data = DateOnly.FromDateTime(DateTime.UtcNow.Date),
                    MediaValorFinal = 1500.75m,
                    QuantidadeSimulacoes = 1270
                }
            };

            _simulacaoRepoMock
                .Setup(x => x.ObterSimulacoesPorProdutoDiaAsync())
                .ReturnsAsync(listaEsperada);

            // Act
            var resultado = await _service.ObterPorProdutoDiaAsync(userId, email);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(listaEsperada, resultado);

            _simulacaoRepoMock.Verify(
                x => x.ObterSimulacoesPorProdutoDiaAsync(),
                Times.Once);
        }
    }
}