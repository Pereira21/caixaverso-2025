using AutoMapper;
using InvestimentosCaixa.Api.Models.Auth;
using InvestimentosCaixa.Application.Interfaces.Services;
using InvestimentosCaixa.Application.Notificacoes;
using Microsoft.AspNetCore.Mvc;

namespace InvestimentosCaixa.Api.Controllers
{
    /// <summary>
    /// Controlador focado em requisições de autenticação
    /// </summary>
    public class AuthController : MainController
    {
        private readonly IAuthService _authService;

        public AuthController(IMapper mapper, INotificador notificador, IConfiguration config, IAuthService authService) : base (mapper, notificador)
        {
            _authService = authService;
        }

        /// <summary>
        /// Retorna credencial para utilizar serviços protegidos da API.
        /// </summary>
        /// <param name="model">Retorna informações de token</param>
        /// <response code="200">Login realizado com sucesso</response>
        /// <response code="400">Não foi possível processar a requisição devido a parâmetros inválidos</response>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var token = await _authService.LoginAsync(model.Email, model.Senha);

            return CustomResponse(new { token });
        }
    }
}
