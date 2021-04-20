using ApplicationCore.Entities;
using System;

namespace Infrastructure.Respositories.Mappings
{
    internal class ProdutoNpsEntityMap : EntityMapConfiguration<ProdutoNpsEntity, Guid>
    {
        protected override void OnMapping()
        {
            Builder.Property(t => t.Email)
                .HasMaxLength(200)
                .IsRequired();

            Builder.Property(t => t.Comentario)
                .HasMaxLength(2000)
                .IsRequired(false);


            Builder.HasOneWithMany(t => t.Produto);
        }

    }
}
