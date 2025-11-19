using AutoMapper;
using InvestimentosCaixa.Application.Interfaces.Repositorios;
using InvestimentosCaixa.Application.Notificacoes;
using InvestimentosCaixa.Application.Resources;
using InvestimentosCaixa.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;

namespace InvestimentosCaixa.Tests
{
    public class AuthServiceTests
    {
        private readonly Mock<UserManager<IdentityUser<Guid>>> _userManagerMock;
        private readonly FakeSignInManager _fakeSignInManager;
        private readonly Mock<IConfiguration> _configMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly Notificador _notificador;
        private readonly AuthService _service;

        public AuthServiceTests()
        {
            _userManagerMock = MockUserManager();
            _fakeSignInManager = new FakeSignInManager(_userManagerMock.Object);

            _configMock = new Mock<IConfiguration>();
            _mapperMock = new Mock<IMapper>();
            _uowMock = new Mock<IUnitOfWork>();
            _notificador = new Notificador();

            // Config mínima para token
            _configMock.Setup(x => x["JwtSettings:Issuer"]).Returns("issuer");
            _configMock.Setup(x => x["JwtSettings:Audience"]).Returns("audience");
            _configMock.Setup(x => x["JwtSettings:Key"]).Returns("chave-super-secreta-de-testes-1234567890");

            _service = new AuthService(
                _notificador,
                _mapperMock.Object,
                _uowMock.Object,
                _configMock.Object,
                _userManagerMock.Object,
                _fakeSignInManager
            );
        }

        [Fact(DisplayName = "Login deve notificar quando usuário não existe")]
        public async Task Login_UsuarioNaoExiste_DeveNotificar()
        {
            // Arrange
            _userManagerMock.Setup(x => x.FindByEmailAsync("email@teste.com"))
                            .ReturnsAsync((IdentityUser<Guid>?)null);

            // Act
            var result = await _service.LoginAsync("email@teste.com", "123");

            // Assert
            Assert.Equal(string.Empty, result);
            Assert.Contains(_notificador.ObterNotificacoes(),
                n => n.Mensagem == Mensagens.UsuarioNaoCadastrado);
        }


        [Fact(DisplayName = "Login deve notificar quando senha está incorreta")]
        public async Task Login_SenhaIncorreta_DeveNotificar()
        {
            // Arrange
            var user = new IdentityUser<Guid>
            {
                Id = Guid.NewGuid(),
                Email = "usuario@teste.com"
            };

            _userManagerMock.Setup(x => x.FindByEmailAsync(user.Email))
                            .ReturnsAsync(user);

            _fakeSignInManager.ResultadoEsperado = SignInResult.Failed;

            // Act
            var result = await _service.LoginAsync(user.Email, "senhaErrada");

            // Assert
            Assert.Equal(string.Empty, result);
            Assert.Contains(_notificador.ObterNotificacoes(),
                n => n.Mensagem == Mensagens.UsuarioOuSenhaInvalidos);
        }


        [Fact(DisplayName = "Login deve retornar token quando credenciais corretas")]
        public async Task Login_Valido_DeveGerarToken()
        {
            // Arrange
            var user = new IdentityUser<Guid>
            {
                Id = Guid.NewGuid(),
                Email = "usuario@teste.com"
            };

            _userManagerMock.Setup(x => x.FindByEmailAsync(user.Email))
                            .ReturnsAsync(user);

            _fakeSignInManager.ResultadoEsperado = SignInResult.Success;

            _userManagerMock.Setup(x => x.GetRolesAsync(user))
                            .ReturnsAsync(new List<string> { "Admin" });

            // Act
            var token = await _service.LoginAsync(user.Email, "123");

            // Assert
            Assert.False(string.IsNullOrEmpty(token));
            Assert.Empty(_notificador.ObterNotificacoes());
        }


        #region metodos privados
        private static Mock<UserManager<IdentityUser<Guid>>> MockUserManager()
        {
            var store = new Mock<IUserStore<IdentityUser<Guid>>>();

            return new Mock<UserManager<IdentityUser<Guid>>>(
                store.Object,
                null, null, null, null, null, null, null, null
            );
        }
        #endregion
    }

    
    /// <summary>
    /// Para fake signin
    /// </summary>
    public class FakeSignInManager : SignInManager<IdentityUser<Guid>>
    {
        public FakeSignInManager(UserManager<IdentityUser<Guid>> userManager) : base(
                  userManager,
                  new Mock<IHttpContextAccessor>().Object,
                  new Mock<IUserClaimsPrincipalFactory<IdentityUser<Guid>>>().Object,
                  null, null, null) { }

        public SignInResult ResultadoEsperado { get; set; } = SignInResult.Failed;

        public override Task<SignInResult> CheckPasswordSignInAsync(
            IdentityUser<Guid> user,
            string password,
            bool lockoutOnFailure)
        {
            return Task.FromResult(ResultadoEsperado);
        }
    }
}
