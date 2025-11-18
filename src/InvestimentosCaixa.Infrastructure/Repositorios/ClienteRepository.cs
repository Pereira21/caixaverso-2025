using InvestimentosCaixa.Application.Interfaces.Repositorios;
using InvestimentosCaixa.Domain.Entidades;
using Microsoft.Extensions.Caching.Distributed;

namespace InvestimentosCaixa.Infrastructure.Repositorios
{
    public class ClienteRepository : Repository<Cliente>, IClienteRepository
    {
        public ClienteRepository(InvestimentosCaixaDbContext context, IDistributedCache distributedCache) : base(context, distributedCache) { }

    }
}
