using InvestimentosCaixa.Domain.Entidades;
using System.Linq.Expressions;

namespace InvestimentosCaixa.Domain.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<T?> ObterPeloIdAsync(int id);
        Task<IEnumerable<T>> ObterTodosAsync();
        Task<IEnumerable<T>> ObterAsync(Expression<Func<T, bool>> predicate);

        Task AdicionarAsync(T entity);
        void Atualizar(T entity);
        void Remover(T entity);
    }
}
