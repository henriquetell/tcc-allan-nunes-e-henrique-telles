namespace ApplicationCore.Entities
{
    public abstract class EntityBase<TKey> : IEntityBase
    {
        public TKey Id { get; set; }
    }

    public abstract class EntityBase : EntityBase<int>
    { }

    public interface IEntityBase { }

}
