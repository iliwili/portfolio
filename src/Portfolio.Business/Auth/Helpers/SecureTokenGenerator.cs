using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;

namespace Portfolio.Business.Auth.Helpers;

public interface ISecureTokenGenerator
{
    string Generate(int size = 32);
    byte[] Hash(string token);
}

public class SecureTokenGenerator : ISecureTokenGenerator
{
    public string Generate(int size = 32)
        => WebEncoders.Base64UrlEncode(
            RandomNumberGenerator.GetBytes(size));

    public byte[] Hash(string token)
        => SHA256.HashData(Encoding.UTF8.GetBytes(token));
}