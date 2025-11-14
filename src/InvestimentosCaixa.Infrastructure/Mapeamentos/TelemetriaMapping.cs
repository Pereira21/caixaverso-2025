using InvestimentosCaixa.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvestimentosCaixa.Infrastructure.Mapeamentos
{
    public class TelemetriaMapping : IEntityTypeConfiguration<Telemetria>
    {
        public void Configure(EntityTypeBuilder<Telemetria> builder)
        {
            builder.ToTable("Telemetria");

            builder.HasKey(x => x.Id);

            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Endpoint)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.Metodo)
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(x => x.TempoRespostaMs)
                .IsRequired();

            builder.Property(x => x.Sucesso)
                .IsRequired();

            builder.Property(x => x.DataRegistro)
                .IsRequired();
        }
    }
}
