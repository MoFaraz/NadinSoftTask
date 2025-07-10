namespace NadinSoft.Domain.Common;

public interface IEntity
{
    DateTime CreatedDate { get; set; }
    DateTime ModifiedDate { get; set; }
}

public class BaseEntity<TKey> : IEntity
{
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }

    public TKey Id { get; set; }
}