using ApplicationCore.Entities;

namespace Infrastructure.Respositories.Mappings
{
    internal class GrupoUsuarioEntityMap : EntityMapConfiguration<GrupoUsuarioEntity>
    {
        protected override void OnMapping()
        {
            Builder.Property(t => t.Nome)
               .IsRequired()
               .HasMaxLength(100);
        }
    }
}
