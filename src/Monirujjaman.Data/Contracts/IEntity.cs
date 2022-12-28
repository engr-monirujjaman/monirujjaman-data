namespace Monirujjaman.Data.Contracts;

public interface IEntity<out TKey>
{
    TKey Id { get; }
}