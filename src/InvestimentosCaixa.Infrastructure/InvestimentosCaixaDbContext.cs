using InvestimentosCaixa.Domain.Entidades;
using InvestimentosCaixa.Infrastructure.Mapeamentos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InvestimentosCaixa.Infrastructure
{
    public class InvestimentosCaixaDbContext : IdentityDbContext<IdentityUser<Guid>, IdentityRole<Guid>, Guid>
    {
        public InvestimentosCaixaDbContext(DbContextOptions<InvestimentosCaixaDbContext> options) : base(options) { }

        public DbSet<Produto> Produtos { get; set; }
        public DbSet<LogTelemetria> LogsTelemetria { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ProdutoMapping());
            modelBuilder.ApplyConfiguration(new TipoProdutoMapping());
            modelBuilder.ApplyConfiguration(new SimulacaoMapping());
            modelBuilder.ApplyConfiguration(new LogTelemetriaMapping());
            modelBuilder.ApplyConfiguration(new PerfilRiscoMapping());
            modelBuilder.ApplyConfiguration(new PerfilPontuacaoVolumeMapping());
            modelBuilder.ApplyConfiguration(new PerfilPontuacaoFrequenciaMapping());
            modelBuilder.ApplyConfiguration(new PerfilPontuacaoRiscoMapping());
            modelBuilder.ApplyConfiguration(new PerfilClassificacaoMapping());
        }
    }
}
