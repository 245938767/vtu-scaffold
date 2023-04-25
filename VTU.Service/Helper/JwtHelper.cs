using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using VTU.Infrastructure;
using VTU.Infrastructure.Extension;
using VTU.Models;
using WebApplication1.Infrastructure.Constant;

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
    /// 生成token
    /// </summary>
    /// <param name="claims"></param>
    /// <param name="jwtSettings"></param>
    /// <returns></returns>
    public static string GenerateJwtToken(List<Claim> claims)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(AppSettingInstants.GetAppSettings().JwtSettings.SecretKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = AppSettingInstants.GetAppSettings().JwtSettings.Issuer,
            Audience = AppSettingInstants.GetAppSettings().JwtSettings.Audience,
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddMinutes(AppSettingInstants.GetAppSettings().JwtSettings.Expire),
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
        var key = Encoding.ASCII.GetBytes(AppSettingInstants.GetAppSettings().JwtSettings.SecretKey);

        var tokenDescriptor = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = false,
            ValidateAudience = false,
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
            var userData = jwtToken.FirstOrDefault(x => x.Type == ClaimTypes.UserData)?.Value;
            var loginUser = JsonConvert.DeserializeObject<LoginUser>(userData);
            var permissions = (List<string>)CacheHelper.GetCache(GlobalConstant.UserPermKey + loginUser.UserId);
            if (loginUser?.UserName == GlobalConstant.AdminRole)
            {
                permissions = new List<string>() { GlobalConstant.AdminPerm };
            }

            if (permissions == null) return null;
            loginUser!.Permissions = permissions;
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
        if (user.Permissions.Count > 50)
        {
            user.Permissions = new List<string>();
        }

        var claims = new List<Claim>()
        {
            new(ClaimTypes.PrimarySid, user.UserId.ToString()),
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.UserData, JsonConvert.SerializeObject(user))
        };

        return claims;
    }
}