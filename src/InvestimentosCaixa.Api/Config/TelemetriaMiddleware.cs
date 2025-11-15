using InvestimentosCaixa.Application.Interfaces.Repositorios;
using InvestimentosCaixa.Domain.Entidades;
using System.Diagnostics;

namespace InvestimentosCaixa.Api.Config
{
    public class TelemetriaMiddleware
    {
        private readonly RequestDelegate _next;

        public TelemetriaMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IUnitOfWork unitOfWork, ILogTelemetriaRepository telemetriaRepository)
        {
            var stopwatch = Stopwatch.StartNew();

            await _next(context);

            stopwatch.Stop();

            var registro = new LogTelemetria(
                endpoint: context.Request.Path,
                metodo: context.Request.Method,
                tempoRespostaMs: (int)stopwatch.ElapsedMilliseconds,
                sucesso: context.Response.StatusCode < 400,
                dataRegistro: DateTime.Now
            );

            await telemetriaRepository.AdicionarAsync(registro);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
