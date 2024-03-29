using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using VTU.Data.Models;
using VTU.Infrastructure;
using VTU.Infrastructure.Constant;
using VTU.Infrastructure.Extension;
using VTU.Models;

namespace VTU.Service.Helper;

public class JwtHelper
{
    /// <summary>
    /// 获取用户身份信息
    /// </summary>
    /// <param name="httpContext"></param>
    /// <returns></returns>
    public static LoginUser? GetLoginUser(HttpContext httpContext)
    {
        var token = httpContext.GetToken();
        if (string.IsNullOrEmpty(token)) return null;
        var enumerable = ParseToken(token);
        return ValidateJwtToken(enumerable);
    }

    /// <summary>
    /// 获取用户身份信息
    /// </summary>
    /// <param name="httpContext"></param>
    /// <returns></returns>
    public static IEnumerable<Claim>? GetLoginUserClaims(HttpContext httpContext)
    {
        var token = httpContext.GetToken();
        return string.IsNullOrEmpty(token) ? null : ParseToken(token);
    }

    /// <summary>
    /// 生成token
    /// </summary>
    /// <param name="claims"></param>
    /// <param name="loginUser"></param>
    /// <returns></returns>
    public static string GenerateJwtToken(List<Claim> claims, LoginUser loginUser)
    {
        var dateTime = DateTime.Now;
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(AppSettingInstants.GetAppSettings().JwtSettings.SecretKey);
        var expires = AppSettingInstants.GetAppSettings().JwtSettings.Expire;
        var issuer = AppSettingInstants.GetAppSettings().JwtSettings.Issuer;
        var audience = AppSettingInstants.GetAppSettings().JwtSettings.Audience;
        claims.Add(new Claim("Audience", audience));
        claims.Add(new Claim("Issuer", issuer));
        //保存缓存
        CacheHelper.SetCache(GlobalConstant.UserPermKey + loginUser.UserId, loginUser, expires);
        //生成token
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = issuer,
            Audience = audience,
            Subject = new ClaimsIdentity(claims),
            IssuedAt = dateTime,
            Expires = dateTime.AddMinutes(expires),
            TokenType = "Bearer",
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    /// <summary>
    /// 验证Token
    /// </summary>
    /// <returns></returns>
    public static TokenValidationParameters ValidParameters()
    {
        var jwtSettings = AppSettingInstants.GetAppSettings().JwtSettings;
        var key = Encoding.ASCII.GetBytes(jwtSettings.SecretKey);

        var tokenDescriptor = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = jwtSettings.Audience,
            ValidIssuer = jwtSettings.Issuer,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromSeconds(30)
        };
        return tokenDescriptor;
    }

    /// <summary>
    /// 从令牌中获取数据声明
    /// </summary>
    /// <param name="token">令牌</param>
    /// <returns></returns>
    private static IEnumerable<Claim>? ParseToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var validateParameter = ValidParameters();
        token = token.Replace("Bearer ", "");
        try
        {
            tokenHandler.ValidateToken(token, validateParameter, out SecurityToken validatedToken);

            var jwtToken = tokenHandler.ReadJwtToken(token);
            return jwtToken.Claims;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    /// <summary>
    /// jwt token校验
    /// </summary>
    /// <param name="jwtToken"></param>
    /// <returns></returns>
    private static LoginUser? ValidateJwtToken(IEnumerable<Claim> jwtToken)
    {
        try
        {
            var userData = jwtToken.FirstOrDefault(x => x.Type == ClaimConstant.PrimarySid)?.Value;
            var loginUser = (LoginUser)CacheHelper.GetCache(GlobalConstant.UserPermKey + userData);
            if (loginUser == null) return null;
            var permissions = loginUser.Permissions;
            if (loginUser.UserName == GlobalConstant.AdminRole)
            {
                permissions = new List<string>() { GlobalConstant.AdminPerm };
            }

            loginUser.Permissions = permissions;
            return loginUser;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    /// <summary>
    ///Claims
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public static List<Claim> AddClaims(LoginUser user)
    {
        var claims = new List<Claim>()
        {
            new(ClaimConstant.PrimarySid, user.UserId.ToString()),
            new(ClaimConstant.Name, user.UserName),
        };

        return claims;
    }
}