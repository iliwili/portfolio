namespace Portfolio.Dal.Utils;

public static class PublicIdGenerator
{
    public static string GeneratePublicId(string prefix)
    {
        var guid = Guid.NewGuid().ToString("N"); // no hyphens
        return $"{prefix}_{guid}";
    }

    public static string GetPrefixFor(Type t) => t.Name switch
    {
        "User" => "usr",
        "Account" => "acc",
        "AccountUser" => "acc_usr",
        "RefreshToken" => "rt",
        "PasswordResetToken" => "prt",
        _ => t.Name.ToLower()
    };
}