namespace Monirujjaman.Data.Contracts;

public interface IEntity<out TKey> where TKey : IComparable<TKey>
{
    TKey Id { get; }
}