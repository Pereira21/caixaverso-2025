using AutoMapper;
using Moq;
using InvestimentosCaixa.Application.Interfaces.Repositorios;
using InvestimentosCaixa.Application.Notificacoes;
using InvestimentosCaixa.Application.Services;
using Microsoft.Extensions.Logging;

namespace InvestimentosCaixa.Application.Tests.Services
{
    public class InvestimentoServiceTests
    {
        private readonly Mock<ILogger<InvestimentoService>> _logMock;
        private readonly Mock<IInvestimentoRepository> _repoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly Mock<INotificador> _notificadorMock;

        private readonly InvestimentoService _service;

        public InvestimentoServiceTests()
        {
            _logMock = new Mock<ILogger<InvestimentoService>>();
            _repoMock = new Mock<IInvestimentoRepository>();
            _mapperMock = new Mock<IMapper>();
            _uowMock = new Mock<IUnitOfWork>();
            _notificadorMock = new Mock<INotificador>();

            _service = new InvestimentoService(
                _notificadorMock.Object,
                _mapperMock.Object,
                _uowMock.Object,
                _repoMock.Object,
                _logMock.Object
            );
        }
    }
}
