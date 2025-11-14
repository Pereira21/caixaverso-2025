using InvestimentosCaixa.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvestimentosCaixa.Infrastructure.Mapeamentos
{
    public class ProdutoMapping : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.ToTable("Produto");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder.Property(p => p.Nome)
                .HasColumnType("VARCHAR(100)")
                .IsRequired();

            builder.Property(p => p.RentabilidadeAnual)
                .HasColumnType("DECIMAL(5,4)")
                .IsRequired();

            builder.Property(p => p.PrazoMinimoMeses)
                .IsRequired();

            builder.Property(p => p.TipoProdutoId)
                .IsRequired();

            builder.HasOne(p => p.TipoProduto)
                .WithMany(tp => tp.Produtos)
                .HasForeignKey(p => p.TipoProdutoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
