namespace InvestimentosCaixa.Application.Interfaces.Repositorios
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
