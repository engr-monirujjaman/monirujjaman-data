namespace Monirujjaman.Data.Contracts;

public interface IEntity<TKey> : IEntityBase
{
    TKey Id { get; set; }
}
