namespace Portfolio.Dal.Utils;

public interface ICurrentUser
{
    public string PublicId { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
}