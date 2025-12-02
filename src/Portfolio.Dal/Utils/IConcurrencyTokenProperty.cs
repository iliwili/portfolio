namespace Portfolio.Dal.Utils;

public interface IConcurrencyTokenProperty
{
    public Guid LockId { get; set; }
}
