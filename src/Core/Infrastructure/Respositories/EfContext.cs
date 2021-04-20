using ApplicationCore.Entities;
using Infrastructure.Respositories.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;

namespace Infrastructure.Respositories
{
    internal partial class EfContext : DbContext
    {
        public EfContext(DbContextOptions<EfContext> options)
            : base(options)
        { }

        public DbSet<ConteudoEntity> Conteudo { get; set; }
        public DbSet<ConteudoAnexoEntity> ConteudoAnexo { get; set; }
        public DbSet<ProdutoEntity> Produto { get; set; }
        public DbSet<ProdutoNpsEntity> ProdutoNps { get; set; }
        public DbSet<GrupoUsuarioEntity> GrupoUsuario { get; set; }
        public DbSet<GrupoUsuarioPermisaoAcaoEntity> GrupoUsuarioPermisaoAcao { get; set; }
        public DbSet<PermissaoAcaoEntity> PermissaoAcao { get; set; }
        public DbSet<PermissaoEntity> Permissao { get; set; }
        public DbSet<UsuarioEntity> Usuario { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var mapTypes = from t in GetType().Assembly.GetTypes()
                           where t.BaseType != null &&
                                 t.BaseType.IsGenericType &&
                                 !t.IsGenericTypeDefinition &&
                                 (t.BaseType.GetGenericTypeDefinition() == typeof(EntityMapConfiguration<>) ||
                                  t.BaseType.GetGenericTypeDefinition() == typeof(EntityMapConfiguration<,>))
                           select t;

            //Adicionando os mapeamentos
            foreach (var mapType in mapTypes)
            {
                var map = Activator.CreateInstance(mapType) as EntityMapConfiguration;
                map?.Mapping(modelBuilder);
            }

            //Alterando o tipo nvarchar par varchar
            foreach (var prop in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetProperties()))
            {
                if (prop.ClrType == typeof(string) && prop.GetColumnType() == null)
                    prop.SetColumnType($"varchar({prop.GetMaxLength()?.ToString() ?? "MAX"})");
            }

            //Padronizando os nomes das PKs
            foreach (var key in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetKeys()))
            {
                key.SetName(key.GetName().Replace("Entity", string.Empty));
            }

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                //Criar index para para FK
                var properties = relationship.Properties.Select(p => p.Name).ToArray();

                modelBuilder.Entity(relationship.DeclaringEntityType.ClrType)
                              .HasIndex(properties)
                              .HasName($"IX_{string.Join("_", properties)}");
            }

        }

        public int SaveChanges(bool ignoreSetEntryStateToReferences = false, bool acceptAllChangesOnSuccess = true)
        {
            SetEntityDateLog();
            if (!ignoreSetEntryStateToReferences)
                SetEntryStateToReferences();

            return base.SaveChanges(acceptAllChangesOnSuccess);
        }


        private void SetEntryStateToReferences()
        {
            var changeSet = ChangeTracker.Entries<IEntityBase>().ToList();
            foreach (var entityEntry in changeSet)
                SetEntryStateToReferences(entityEntry);
        }

        private void SetEntryStateToReferences(EntityEntry entityEntry)
        {
            var refs = entityEntry.References.Where(r => r.TargetEntry != null && r.CurrentValue?.GetType().GetInterface(typeof(IEntityBase).FullName) == null);
            foreach (var i in refs)
            {
                i.TargetEntry.State = entityEntry.State;
                SetEntryStateToReferences(i.TargetEntry);
            }
        }

        private void SetEntityDateLog()
        {
            var changeSet = ChangeTracker.Entries<IEntityDateLog>();
            foreach (var entityEntry in changeSet)
                SetEntityDateLog(entityEntry);
        }

        private static void SetEntityDateLog<TEntityDateLog>(EntityEntry<TEntityDateLog> entityEntry) where TEntityDateLog : class
        {
            if (!(entityEntry.Entity is IEntityDateLog)) return;

            if (entityEntry.State == EntityState.Added)
            {
                var dataCriacao = entityEntry.Property("DataCriacao");
                if (((DateTime)dataCriacao.CurrentValue) == DateTime.MinValue)
                    entityEntry.Property("DataCriacao").CurrentValue = DateTime.Now;
            }
            else if (entityEntry.State == EntityState.Modified)
            {
                entityEntry.Property("DataAtualizacao").CurrentValue = DateTime.Now;
                entityEntry.Property("DataCriacao").IsModified = false;
            }
        }
    }
}
