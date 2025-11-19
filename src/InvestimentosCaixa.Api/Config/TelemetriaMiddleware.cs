using InvestimentosCaixa.Application.DTO.Request;
using InvestimentosCaixa.Application.Interfaces.Repositorios;
using InvestimentosCaixa.Application.Interfaces.Services;
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

        public async Task Invoke(HttpContext context, IUnitOfWork unitOfWork, ILogTelemetriaService logTelemetriaService)
        {
            var stopwatch = Stopwatch.StartNew();

            await _next(context);

            stopwatch.Stop();

            var registro = new LogTelemetriaRequest()
            {
                Endpoint = context.Request.Path.Value?.TrimStart('/'),
                Metodo = context.Request.Method,
                TempoRespostaMs = (int)stopwatch.ElapsedMilliseconds,
                Sucesso = context.Response.StatusCode < 400,
                DataRegistro = DateTime.UtcNow
            };

            await logTelemetriaService.AdicionarAsync(registro);
        }
    }
}
