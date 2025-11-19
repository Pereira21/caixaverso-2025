using InvestimentosCaixa.Api;
using InvestimentosCaixa.Api.Controllers;
using InvestimentosCaixa.Application.Interfaces.Services;
using InvestimentosCaixa.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;

namespace InvestimentosCaixa.Tests.Integracao.Helpers
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IDisposable
    {
        private readonly string _dbName = $"InvestimentosCaixa_Test_{Guid.NewGuid()}";

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Substitui o DbContext real
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<InvestimentosCaixaDbContext>));
                if (descriptor != null) services.Remove(descriptor);

                services.AddDbContext<InvestimentosCaixaDbContext>(options =>
                    options.UseSqlServer($"Server=localhost,1433;Database={_dbName};User Id=sa;Password=SenhaF0rte!2025;TrustServerCertificate=True;"));

                // Remove implementação real do IAuthService
                var authDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IAuthService));
                if (authDescriptor != null) services.Remove(authDescriptor);

                // Mock do AuthService
                var mockAuthService = new Mock<IAuthService>();
                mockAuthService.Setup(x => x.LoginAsync("usuario@teste.com", "SenhaForte123!"))
                               .ReturnsAsync("token_fake");

                services.AddSingleton(mockAuthService.Object);

                // Mock do Redis (IDistributedCache)
                var mockCache = new Mock<IDistributedCache>();
                mockCache.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync((byte[])null); // cache vazio
                mockCache.Setup(x => x.SetAsync(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()))
                         .Returns(Task.CompletedTask);

                // Remove cache real e adiciona mock
                var cacheDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IDistributedCache));
                if (cacheDescriptor != null) services.Remove(cacheDescriptor);
                services.AddSingleton(mockCache.Object);

                // Configura autenticação fake
                services.AddAuthentication("TestAuth")
                        .AddScheme<AuthenticationSchemeOptions, CustomAuthHandler>("TestAuth", options => { });

                services.PostConfigure<AuthenticationOptions>(opts =>
                {
                    opts.DefaultAuthenticateScheme = "TestAuth";
                    opts.DefaultChallengeScheme = "TestAuth";
                });

                // Adiciona controllers
                services.AddControllers().AddApplicationPart(typeof(TelemetriasController).Assembly);

                // substituir IPerfilRiscoService por um mock configurável
                var mockPerfilRiscoService = new Mock<IPerfilRiscoService>();
                var mockSimulacaoService = new Mock<ISimulacaoService>();

                // Cria o banco de teste
                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<InvestimentosCaixaDbContext>();
                db.Database.Migrate();
            });

            builder.Configure(app =>
            {
                app.UseRouting();
                app.UseAuthentication();
                app.UseAuthorization();

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
            });
        }

        public new void Dispose()
        {
            try
            {
                using var scope = Services.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<InvestimentosCaixaDbContext>();
                db.Database.EnsureDeleted(); // Deleta o banco dinâmico criado
            }
            catch
            {
                // Ignora erros de exclusão, caso o banco já tenha sido removido
            }

            base.Dispose();
        }
    }
}
