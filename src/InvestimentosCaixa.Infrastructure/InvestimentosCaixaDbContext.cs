using InvestimentosCaixa.Domain.Entidades;
using InvestimentosCaixa.Infrastructure.Mapeamentos;
using Microsoft.EntityFrameworkCore;

namespace InvestimentosCaixa.Infrastructure
{
    public class InvestimentosCaixaDbContext : DbContext
    {
        public InvestimentosCaixaDbContext(DbContextOptions<InvestimentosCaixaDbContext> options)
            : base(options)
        {
        }

        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Telemetria> Telemetrias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ProdutoMapping());
            modelBuilder.ApplyConfiguration(new TipoProdutoMapping());
            modelBuilder.ApplyConfiguration(new SimulacaoMapping());
            modelBuilder.ApplyConfiguration(new TelemetriaMapping());
        }
    }
}
