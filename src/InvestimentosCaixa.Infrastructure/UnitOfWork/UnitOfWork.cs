using InvestimentosCaixa.Domain.Repositorios;

namespace InvestimentosCaixa.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly InvestimentosCaixaDbContext _context;
        public UnitOfWork(InvestimentosCaixaDbContext context) => _context = context;

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => _context.SaveChangesAsync(cancellationToken);
    }
}
