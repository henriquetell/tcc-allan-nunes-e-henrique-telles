using ApplicationCore.Entities;

namespace Infrastructure.Respositories.Mappings
{
    internal class ConteudoAnexoEntityMap : EntityMapConfiguration<ConteudoAnexoEntity>
    {
        protected override void OnMapping()
        {
            Builder.Property(t => t.Anexo)
                .HasMaxLength(500)
                .IsRequired();

            Builder.Property(t => t.NomeOriginal)
                .HasMaxLength(500)
                .IsRequired();

            Builder.HasOneWithMany(t => t.Conteudo);
        }
    }
}
