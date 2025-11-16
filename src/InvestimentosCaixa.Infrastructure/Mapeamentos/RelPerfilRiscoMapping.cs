using InvestimentosCaixa.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvestimentosCaixa.Infrastructure.Mapeamentos
{
    public class RelPerfilRiscoMapping : IEntityTypeConfiguration<RelPerfilRisco>
    {
        public void Configure(EntityTypeBuilder<RelPerfilRisco> builder)
        {
            builder.ToTable("RelPerfilRisco");

            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder.Property(p => p.PerfilRiscoId)
                .IsRequired();

            builder.Property(p => p.RiscoId)
                .IsRequired();

            builder.HasOne(p => p.PerfilRisco)
                .WithMany(p => p.RelPerfilRiscoList)
                .HasForeignKey(p => p.PerfilRiscoId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.Risco)
                .WithMany()
                .HasForeignKey(p => p.RiscoId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(p => new { p.PerfilRiscoId, p.RiscoId })
                .IsUnique();
        }
    }
}
