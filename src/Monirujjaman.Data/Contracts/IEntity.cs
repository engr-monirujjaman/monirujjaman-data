namespace Monirujjaman.Data.Contracts;

public interface IEntity<TKey> 
{
    TKey Id { get; set; }
}
