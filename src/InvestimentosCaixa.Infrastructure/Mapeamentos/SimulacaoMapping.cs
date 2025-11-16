using InvestimentosCaixa.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvestimentosCaixa.Infrastructure.Mapeamentos
{
    public class SimulacaoMapping : IEntityTypeConfiguration<Simulacao>
    {
        public void Configure(EntityTypeBuilder<Simulacao> builder)
        {
            builder.ToTable("Simulacao");

            builder.HasKey(s => s.Id);

            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder.Property(s => s.ClienteId)
                .IsRequired();

            builder.Property(s => s.ProdutoId)
                .IsRequired();

            builder.Property(s => s.ValorInvestido)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(s => s.ValorFinal)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(s => s.PrazoMeses)
                .IsRequired();

            builder.Property(s => s.RentabilidadeEfetiva)
                .HasColumnType("decimal(5,4)")
                .IsRequired();

            builder.Property(s => s.DataSimulacao)
                .HasColumnType("datetime2")
                .IsRequired();

            builder.HasOne(s => s.Produto)
                .WithMany(p => p.Simulacoes)
                .HasForeignKey(s => s.ProdutoId);
        }
    }
}
