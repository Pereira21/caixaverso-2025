using InvestimentosCaixa.Application.Interfaces.Repositorios;
using InvestimentosCaixa.Domain.Entidades;

namespace InvestimentosCaixa.Infrastructure.Repositorios
{
    public class ClienteRepository : Repository<Cliente>, IClienteRepository
    {
        public ClienteRepository(InvestimentosCaixaDbContext context) : base(context) { }

    }
}
