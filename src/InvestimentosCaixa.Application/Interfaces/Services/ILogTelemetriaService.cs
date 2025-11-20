
using InvestimentosCaixa.Application.DTO.Request;
using InvestimentosCaixa.Application.DTO.Response;

namespace InvestimentosCaixa.Application.Interfaces.Services
{
    public interface ILogTelemetriaService
    {
        /// <summary>
        /// Adicionar log de telemetria
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task AdicionarAsync(LogTelemetriaRequest request);

        /// <summary>
        /// Obter logs de telemetria em um período mensal, paginados
        /// </summary>
        /// <param name="userId">Usuário que está realizando a consulta</param>
        /// <param name="userEmail">E-mail do usuário que está realizando a consulta</param>
        /// <param name="pagina"></param>
        /// <param name="tamanhoPagina"></param>
        /// <returns></returns>
        Task<List<TelemetriaResponse>> ObterPeriodoMensalAsync(Guid userId, string userEmail, int pagina, int tamanhoPagina);
    }
}
