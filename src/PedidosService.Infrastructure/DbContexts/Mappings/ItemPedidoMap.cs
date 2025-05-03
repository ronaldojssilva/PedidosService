using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PedidosService.Domain.Entities;

namespace PedidosService.Infrastructure.DbContexts.Mappings
{
    public class ItemPedidoMap : IEntityTypeConfiguration<ItemPedido>
    {
        public void Configure(EntityTypeBuilder<ItemPedido> builder)
        {
            builder.ToTable("ItensPedido");

            builder.HasKey(i => i.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.HasOne(p => p.Pedido)
                .WithMany(p => p.Itens)
                .HasForeignKey(p => p.PedidoId) 
                .IsRequired();

            builder.Property(i => i.ProdutoId)
                .IsRequired();

            builder.Property(i => i.Quantidade)
                .IsRequired();

            builder.Property(i => i.ValorUnitario)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
        }
    }
}
