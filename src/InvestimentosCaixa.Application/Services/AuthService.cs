using AutoMapper;
using InvestimentosCaixa.Application.Interfaces.Repositorios;
using InvestimentosCaixa.Application.Interfaces.Services;
using InvestimentosCaixa.Application.Notificacoes;
using InvestimentosCaixa.Application.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace InvestimentosCaixa.Application.Services
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<IdentityUser<Guid>> _userManager;
        private readonly SignInManager<IdentityUser<Guid>> _signInManager;

        public AuthService(INotificador notificador, IMapper mapper, IUnitOfWork unitOfWork, IConfiguration configuration, UserManager<IdentityUser<Guid>> userManager, SignInManager<IdentityUser<Guid>> signInManager) : 
            base(notificador, mapper, unitOfWork)
        {
            _configuration = configuration;

            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            var identityUser = await _userManager.FindByEmailAsync(email);
            if (identityUser == null)
            {
                Notificar(Mensagens.UsuarioNaoCadastrado);
                return string.Empty;
            }

            var result = await _signInManager.CheckPasswordSignInAsync(identityUser, password, false);
            if (!result.Succeeded)
            {
                Notificar(Mensagens.UsuarioOuSenhaInvalidos);
                return string.Empty;
            }

            return await GerarTokenJWT(identityUser);
        }

        #region metodos privados
        private async Task<string> GerarTokenJWT(IdentityUser<Guid> user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };

            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                claims.Add(new Claim("role", role));
            }

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
