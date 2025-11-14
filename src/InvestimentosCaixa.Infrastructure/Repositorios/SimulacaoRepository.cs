using InvestimentosCaixa.Domain.Entidades;
using InvestimentosCaixa.Domain.Repositorios;
using InvestimentosCaixa.Infrastructure.DTO;
using Microsoft.EntityFrameworkCore;

namespace InvestimentosCaixa.Infrastructure.Repositorios
{
    public class SimulacaoRepository : Repository<Simulacao>, ISimulacaoRepository
    {
        public SimulacaoRepository(InvestimentosCaixaDbContext context) : base(context) { }

        public async Task<List<Simulacao>> ObterTodosComProdutoAsync()
        {
            return await _dbSet.Include(x => x.Produto).ToListAsync();
        }
    }
}
