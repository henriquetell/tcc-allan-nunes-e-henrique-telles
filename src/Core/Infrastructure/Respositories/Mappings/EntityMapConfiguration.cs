using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Linq.Expressions;

namespace Infrastructure.Respositories.Mappings
{
    internal abstract class EntityMapConfiguration<TEntity> : EntityMapConfiguration<TEntity, int> where TEntity : EntityBase
    {
        
    }

    internal abstract class EntityMapConfiguration<TEntity, TKey> : EntityMapConfiguration where TEntity : EntityBase<TKey>
    {
        protected EntityTypeBuilder<TEntity> Builder { get; private set; }

        protected abstract void OnMapping();

        internal override void Mapping(ModelBuilder modelBuilder)
        {
            Builder = modelBuilder.Entity<TEntity>();

            if (typeof(IEntityDateLog).IsAssignableFrom(typeof(TEntity)))
            {
                Builder.Property<DateTime>("DataCriacao")
                    .IsRequired();

                Builder.Property<DateTime?>("DataAtualizacao");
            }

            //Padronizando os nomes das tabelas
            Builder.ToTable(typeof(TEntity).Name.Replace("Entity", string.Empty));

            Builder.HasKey(nameof(EntityBase.Id));

            OnMapping();
        }   
    }

    internal abstract class EntityMapConfiguration
    {
        internal abstract void Mapping(ModelBuilder modelBuilder);
    }

    internal static class EntityTypeBuilderExtender
    {
        public static void HasOneWithMany<TEntity, TRelatedEntity>(this EntityTypeBuilder<TEntity> builder, Expression<Func<TEntity, TRelatedEntity>> navigationExpression,
            DeleteBehavior referentialActionOnDelete = DeleteBehavior.Restrict)
            where TEntity : class, IEntityBase
            where TRelatedEntity : class, IEntityBase
        {
            builder.HasOne(navigationExpression)
                .WithMany(typeof(TEntity).EntityName())
                .HasForeignKey("Id" + typeof(TRelatedEntity).EntityName())
                .OnDelete(referentialActionOnDelete);
        }

        private static string EntityName(this Type type) => type.Name.Replace("Entity", "");
    }
}
