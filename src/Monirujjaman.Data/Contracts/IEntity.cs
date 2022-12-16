namespace Monirujjaman.Data.Contracts;

public interface IEntity<out TKey> : IEntityBase
{
    TKey Id { get; }
}
