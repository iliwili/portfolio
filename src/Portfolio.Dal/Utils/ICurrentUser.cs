namespace Portfolio.Dal.Utils;

public interface ICurrentUser
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
}