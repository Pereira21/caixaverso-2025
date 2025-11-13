using InvestimentosCaixa.Application.Interfaces;
using InvestimentosCaixa.Application.Services;

namespace InvestimentosCaixa.Api.Config
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddSingleton<IConfiguration>(configuration);

            //services.AddScoped<INotificador, Notificador>();

            ////Repositorios
            //services.AddScoped<IProdutoRepository, ProdutoRepository>();
            //services.AddScoped<ISimulacaoRepository, SimulacaoRepository>();

            //Servicos
            services.AddScoped<ISimulacaoService, SimulacaoService>();

            ////EventHub
            //services.AddSingleton<IEventHubProducer, EventHubProducerService>();

            return services;
        }
    }
}
