using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace VTU.Infrastructure.Helper;

/// <summary>
/// 密码加密工具
/// </summary>
public static class PasswordHelper
{
    public static string EncryptPassword(string password, string salt)
    {
        return EncryptPassword_Pdkdf2(password, salt);
    }

    /// <summary>
    /// 生成盐
    /// </summary>
    /// <returns></returns>
    public static string GenerateSalt()
    {
        var salt = new byte[128 / 8];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        return Convert.ToBase64String(salt);
    }

    /// <summary>
    /// 加密
    /// </summary>
    /// <param name="password">密码明文</param>
    /// <param name="salt">盐</param>
    /// <returns></returns>
    public static string EncryptPassword_Pdkdf2(string password, string salt)
    {
        return Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: Convert.FromBase64String(salt),
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 256 / 8
        ));
    }
}