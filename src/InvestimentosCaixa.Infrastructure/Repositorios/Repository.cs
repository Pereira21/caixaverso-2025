using InvestimentosCaixa.Application.Interfaces.Repositorios;
using InvestimentosCaixa.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace InvestimentosCaixa.Infrastructure.Repositorios
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly InvestimentosCaixaDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(InvestimentosCaixaDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task AdicionarAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Atualizar(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Remover(T entity)
        {
            _dbSet.Remove(entity);
        }

        public virtual async Task<T?> ObterPeloIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<IEnumerable<T>> ObterAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> ObterTodosAsync()
        {
            return await _dbSet.ToListAsync();
        }
    }
}
