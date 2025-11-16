using AutoMapper;
using FluentAssertions;
using Moq;
using InvestimentosCaixa.Application.Interfaces.Repositorios;
using InvestimentosCaixa.Application.Notificacoes;
using InvestimentosCaixa.Application.Services;
using InvestimentosCaixa.Domain.Entidades;
using InvestimentosCaixa.Application.DTO.Response;

namespace InvestimentosCaixa.Application.Tests.Services
{
    public class InvestimentoServiceTests
    {
        private readonly Mock<IInvestimentoRepository> _repoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly Mock<INotificador> _notificadorMock;

        private readonly InvestimentoService _service;

        public InvestimentoServiceTests()
        {
            _repoMock = new Mock<IInvestimentoRepository>();
            _mapperMock = new Mock<IMapper>();
            _uowMock = new Mock<IUnitOfWork>();
            _notificadorMock = new Mock<INotificador>();

            _service = new InvestimentoService(
                _notificadorMock.Object,
                _mapperMock.Object,
                _uowMock.Object,
                _repoMock.Object
            );
        }

        [Fact]
        public async Task ObterPorClienteId_DeveRetornarListaMapeada_QuandoExistemInvestimentos()
        {
            // Arrange
            int clienteId = 10;

            var investimentos = new List<Investimento>
            {
                new Investimento(1, clienteId, 1, 1000, 0.12m, new DateTime(2025, 09, 16)),
                new Investimento(2, clienteId, 1, 2000, 0.12m, new DateTime(2025, 11, 16))
            };

            var responseMapeado = new List<InvestimentoResponse>
            {
                new InvestimentoResponse { Id = 1, Valor = 1000 },
                new InvestimentoResponse { Id = 2, Valor = 2000 }
            };

            _repoMock
                .Setup(r => r.ObterComProdutoPorClienteId(clienteId))
                .ReturnsAsync(investimentos);

            _mapperMock
                .Setup(m => m.Map<List<InvestimentoResponse>>(investimentos))
                .Returns(responseMapeado);

            // Act
            var result = await _service.ObterPorClienteId(clienteId);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(responseMapeado);

            _repoMock.Verify(r => r.ObterComProdutoPorClienteId(clienteId), Times.Once);
        }

        [Fact]
        public async Task ObterPorClienteId_DeveRetornarListaVazia_QuandoNaoExistemInvestimentos()
        {
            // Arrange
            int clienteId = 20;

            var investimentos = new List<Investimento>();
            var responseVazio = new List<InvestimentoResponse>();

            _repoMock
                .Setup(r => r.ObterComProdutoPorClienteId(clienteId))
                .ReturnsAsync(investimentos);

            _mapperMock
                .Setup(m => m.Map<List<InvestimentoResponse>>(investimentos))
                .Returns(responseVazio);

            // Act
            var result = await _service.ObterPorClienteId(clienteId);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();

            _repoMock.Verify(r => r.ObterComProdutoPorClienteId(clienteId), Times.Once);
        }

        [Fact]
        public async Task ObterPorClienteId_DeveChamarMapperCorretamente()
        {
            // Arrange
            int clienteId = 30;

            var investimentos = new List<Investimento>
            {
                new Investimento(1, clienteId, 1, 1500, 0.40m, new DateTime(2025, 09, 16))
            };

            var response = new List<InvestimentoResponse>
            {
                new InvestimentoResponse { Id = 1, Valor = 1500 }
            };

            _repoMock
                .Setup(r => r.ObterComProdutoPorClienteId(clienteId))
                .ReturnsAsync(investimentos);

            _mapperMock
                .Setup(m => m.Map<List<InvestimentoResponse>>(investimentos))
                .Returns(response);

            // Act
            var result = await _service.ObterPorClienteId(clienteId);

            // Assert
            _mapperMock.Verify(m => m.Map<List<InvestimentoResponse>>(investimentos), Times.Once);
        }
    }
}
