using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Respositories.Mappings
{
    internal class ConteudoEntityMap : EntityMapConfiguration<ConteudoEntity>
    {
        protected override void OnMapping()
        {
            Builder.Property(t => t.Descricao)
                .HasColumnType("varchar(max)")
                .IsRequired();

            Builder.Property(t => t.DescricaoComplemento)
                .HasColumnType("varchar(max)")
                .IsRequired(false);

            Builder.Property(t => t.Assunto)
                .HasColumnType("varchar(500)")
                .IsRequired(false);

            Builder.Property(t => t.Titulo)
                .HasMaxLength(500)
                .IsRequired();

            Builder.Ignore(t => t.ConteudoChave);
        }
    }
}
