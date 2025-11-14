namespace InvestimentosCaixa.Domain.Repositorios
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
