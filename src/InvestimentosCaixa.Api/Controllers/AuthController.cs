using AutoMapper;
using InvestimentosCaixa.Api.Models.Auth;
using InvestimentosCaixa.Application.Notificacoes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace InvestimentosCaixa.Api.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : MainController
    {
        private readonly IConfiguration _configuration;

        public AuthController(IMapper mapper, INotificador notificador, IConfiguration config) : base (mapper, notificador)
        {
            _configuration = config;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var token = GerarTokenJWT(model.UserName);
            return CustomResponse(new { token });
        }

        #region metodos privados
        private string GerarTokenJWT(string userName)
        {
            var claims = new List<Claim>
{
                new Claim("username", userName)
            };

            var issuer = _configuration["JwtSettings:Issuer"];
            var audience = _configuration["JwtSettings:Audience"];
            var expiry = DateTime.Now.AddMinutes(120);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(issuer: issuer, audience: audience, expires: expiry, claims: claims, signingCredentials: credentials);
            var tokenHandler = new JwtSecurityTokenHandler();
            var stringToken = tokenHandler.WriteToken(token);

            return stringToken;
        }
        #endregion
    }
}
