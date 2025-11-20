using InvestimentosCaixa.Application.Interfaces.Repositorios;

namespace InvestimentosCaixa.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly InvestimentosCaixaDbContext _context;
        public UnitOfWork(InvestimentosCaixaDbContext context) => _context = context;

        public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();
    }
}
