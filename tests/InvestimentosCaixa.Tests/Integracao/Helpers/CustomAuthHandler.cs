using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace InvestimentosCaixa.Tests.Integracao.Helpers
{
    public class CustomAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public CustomAuthHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        { }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // Se não tiver header X-Test-User, retorna sem autenticação
            if (!Request.Headers.ContainsKey("X-Test-User"))
                return Task.FromResult(AuthenticateResult.NoResult());

            var headerValue = Request.Headers["X-Test-User"].ToString();
            // Formato esperado: "role=tecnico;email=usuario@tecnico.com;id=GUID"
            var parts = headerValue.Split(';');
            var id = parts.FirstOrDefault(p => p.StartsWith("id="))?.Replace("id=", "") ?? Guid.NewGuid().ToString();
            var email = parts.FirstOrDefault(p => p.StartsWith("email="))?.Replace("email=", "user@test.com");
            var role = parts.FirstOrDefault(p => p.StartsWith("role="))?.Replace("role=", null);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, id),
                new Claim(ClaimTypes.Email, email ?? "user@test.com")
            };

            if (!string.IsNullOrEmpty(role))
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var identity = new ClaimsIdentity(claims, "TestAuth");
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, "TestAuth");

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}