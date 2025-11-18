using InvestimentosCaixa.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvestimentosCaixa.Infrastructure.Mapeamentos
{
    public class LogTelemetriaMapping : IEntityTypeConfiguration<LogTelemetria>
    {
        public void Configure(EntityTypeBuilder<LogTelemetria> builder)
        {
            builder.ToTable("LogTelemetria");

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
