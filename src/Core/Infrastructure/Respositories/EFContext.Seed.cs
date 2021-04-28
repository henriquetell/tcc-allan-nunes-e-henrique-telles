using ApplicationCore.Entities;
using ApplicationCore.Enuns;
using Framework.Security.Authorization;
using Framework.Security.Permissoes;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;

namespace Infrastructure.Respositories
{
    partial class EfContext
    {
        public void Seed()
        {
            SeedPermissao();
            SeedGrupoAcessoUsuarioAdmin();
            SaveChanges();
        }

        #region SeedPermissao e SeedGrupoAcessoUsuarioAdmin

        private void SeedGrupoAcessoUsuarioAdmin()
        {
            var grupoAcesso = SeedEntity(new GrupoUsuarioEntity
            {
                Nome = "Administradores",
                Status = EStatus.Ativo
            }, new[] { nameof(GrupoUsuarioEntity.Nome) }, forcarAtualizacao: false, recuperarSeExistir: true);

            var salt = new byte[128 / 8];

            using (var rng = RandomNumberGenerator.Create())
                rng.GetBytes(salt);

            var usrAdmin = SeedEntity(new UsuarioEntity
            {
                GrupoUsuario = grupoAcesso,
                Status = EStatus.Ativo,
                Email = "admin@admin.com",
                Nome = "Administrador",
                Salt = salt,
                Senha = Convert.ToBase64String(KeyDerivation.Pbkdf2("123456", salt, KeyDerivationPrf.HMACSHA1, 10000, 256 / 8))
            }, new[] { nameof(UsuarioEntity.Email) }, forcarAtualizacao: false, recuperarSeExistir: true);

            var permissoesAcoes = new[] {
                GrupoUsuarioPermissoes.Permitir
            };

            foreach (var permissao in permissoesAcoes)
            {
                SeedEntity(new GrupoUsuarioPermisaoAcaoEntity
                {
                    GrupoUsuario = grupoAcesso,
                    IdPermissaoAcao = new Guid(permissao.Key)
                }, new[] { nameof(GrupoUsuarioPermisaoAcaoEntity.IdGrupoUsuario), nameof(GrupoUsuarioPermisaoAcaoEntity.IdPermissaoAcao) }, false);
            }
        }

        private void SeedPermissao()
        {
            var idsComparacao = new[] { nameof(PermissaoAcaoEntity.Id), nameof(PermissaoAcaoEntity.IdPermissao) };

            SeedEntity(new PermissaoEntity
            {
                Id = new Guid(GrupoUsuarioPermissoes.Gerenciar),
                Descricao = GrupoUsuarioPermissoes.Descricao,
                NomeGrupo = GrupoUsuarioPermissoes.NomeGrupo
            }, false);
            SeedEntity(new PermissaoAcaoEntity
            {
                IdPermissao = new Guid(GrupoUsuarioPermissoes.Gerenciar),
                Id = new Guid(GrupoUsuarioPermissoes.Permitir.Key),
                TipoAcao = (int)AuthPermissaoTipoAcao.Permitir
            }, idsComparacao, false);
        }

        #endregion
        #region SeedEntity

        protected void SeedEntity<TEntity>(TEntity entity, bool forcarAtualizacao = true, string propriedadeComparacao = null)
            where TEntity : class
        {
            SeedEntity(entity, string.IsNullOrWhiteSpace(propriedadeComparacao) ? null : new[] { propriedadeComparacao }, forcarAtualizacao);
        }

        protected TEntity SeedEntity<TEntity>(TEntity entity, string[] propriedadeComparacao, bool forcarAtualizacao = true, bool recuperarSeExistir = false)
           where TEntity : class
        {
            var dbSet = Set<TEntity>();

            var entityType = Model.FindEntityType(typeof(TEntity));
            propriedadeComparacao ??= new[] { entityType.FindPrimaryKey().Properties[0].Name };
            var parameter = Expression.Parameter(typeof(TEntity), "x");

            var consulta = dbSet.AsQueryable();

            foreach (var item in propriedadeComparacao)
            {
                consulta = consulta.Where((Expression<Func<TEntity, bool>>)
                    Expression.Lambda(
                        Expression.Equal(
                            Expression.Property(parameter, item),
                            Expression.Constant(typeof(TEntity).GetProperty(item).GetValue(entity))),
                        parameter));
            }

            if (!consulta.Any())
            {
                var entry = Entry(entity);
                entry.State = EntityState.Added;
                if (entity is IEntityDateLog)
                    SetEntityDateLog(entry);
            }
            else
            {
                if (forcarAtualizacao)
                {
                    var entry = Entry(entity);
                    entry.State = EntityState.Modified;
                    if (entity is IEntityDateLog)
                        SetEntityDateLog(entry);
                }
                else if (recuperarSeExistir)
                {
                    entity = consulta.First();
                }
            }

            return entity;
        }

        #endregion
    }
}
