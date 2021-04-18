using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Respositories.Mappings
{
    internal class UsuarioEntityMap : EntityMapConfiguration<UsuarioEntity>
    {
        protected override void OnMapping()
        {
            Builder.Property(t => t.Nome)
               .IsRequired()
               .HasMaxLength(70);

            Builder.Property(t => t.Email)
                .IsRequired()
                .HasMaxLength(150);

            Builder.Property(t => t.Senha)
                .IsRequired()
                .HasMaxLength(5000);

            Builder.Property(t => t.Salt)
                .IsRequired()
                .HasMaxLength(500);

            Builder.Property(t => t.Imagem)
                .HasColumnType("VARCHAR(MAX)")
                .IsRequired(false);

            Builder.HasOneWithMany(t => t.GrupoUsuario);
        }
    }
}
