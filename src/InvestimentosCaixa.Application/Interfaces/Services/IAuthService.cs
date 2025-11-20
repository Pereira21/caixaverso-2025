namespace InvestimentosCaixa.Application.Interfaces.Services
{
    public interface IAuthService
    {
        /// <summary>
        /// Realiza login e retorna um token JWT
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<string> LoginAsync(string username, string password);
    }
}
