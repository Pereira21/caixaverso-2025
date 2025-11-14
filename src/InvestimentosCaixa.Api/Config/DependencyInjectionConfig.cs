using InvestimentosCaixa.Application.Interfaces;
using InvestimentosCaixa.Application.Notificacoes;
using InvestimentosCaixa.Application.Services;
using InvestimentosCaixa.Domain.Interfaces;
using InvestimentosCaixa.Domain.Repositorios;
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

            // Unit of work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //Servicos
            services.AddScoped<ISimulacaoService, SimulacaoService>();

            ////EventHub
            //services.AddSingleton<IEventHubProducer, EventHubProducerService>();

            return services;
        }
    }
}
