using AutoMapper;
using InvestimentosCaixa.Api.Models.Auth;
using InvestimentosCaixa.Application.Interfaces.Services;
using InvestimentosCaixa.Application.Notificacoes;
using Microsoft.AspNetCore.Mvc;

namespace InvestimentosCaixa.Api.Controllers
{
    public class AuthController : MainController
    {
        private readonly IAuthService _authService;

        public AuthController(IMapper mapper, INotificador notificador, IConfiguration config, IAuthService authService) : base (mapper, notificador)
        {

            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var token = await _authService.LoginAsync(model.Email, model.Senha);

            return CustomResponse(new { token });
        }
    }
}
