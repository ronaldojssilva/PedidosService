using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PedidosService.Domain.Entities;

namespace PedidosService.Infrastructure.DbContexts.Mappings
{
    public class PedidoMap : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {

            builder.ToTable("Pedidos");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Property(p => p.PedidoId).IsRequired();

            builder.Property(p => p.ClienteId).IsRequired();

            builder.HasMany(p => p.Itens)
                   .WithOne(i => i.Pedido)
                   .HasForeignKey(i => i.PedidoId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(p => p.Status)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(p => p.CriadoEm)
                .IsRequired();

            builder.Property(i => i.TotalImpostos)
                .HasColumnType("decimal(18,2)")
            .IsRequired();

            builder.Property(p => p.EnviadoSistemaB)
                .HasDefaultValue(false);

            builder.Navigation(p => p.Itens).AutoInclude();
        }
    }
}
