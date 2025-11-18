using InvestimentosCaixa.Application.Interfaces.Repositorios;
using InvestimentosCaixa.Application.Interfaces.Services;
using InvestimentosCaixa.Application.Notificacoes;
using InvestimentosCaixa.Application.Services;
using InvestimentosCaixa.Infrastructure.Repositorios;
using InvestimentosCaixa.Infrastructure.UnitOfWork;

namespace InvestimentosCaixa.Api.Config
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddSingleton<IConfiguration>(configuration);

            services.AddScoped<INotificador, Notificador>();

            ////Repositorios
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddScoped<ISimulacaoRepository, SimulacaoRepository>();
            services.AddScoped<ILogTelemetriaRepository, LogTelemetriaRepository>();
            services.AddScoped<IPerfilRiscoRepository, PerfilRiscoRepository>();
            services.AddScoped<IInvestimentoRepository, InvestimentoRepository>();
            services.AddScoped<IClienteRepository, ClienteRepository>();

            // Unit of work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //Servicos
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ISimulacaoService, SimulacaoService>();
            services.AddScoped<IPerfilRiscoService, PerfilRiscoService>();
            services.AddScoped<IInvestimentoService, InvestimentoService>();
            services.AddScoped<ILogTelemetriaService, LogTelemetriaService>();

            return services;
        }
    }
}
