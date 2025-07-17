namespace NadinSoft.Domain.Common;

public interface IEntity
{
}

public class BaseEntity<TKey> : IEntity
{
    public TKey Id { get; set; }
}