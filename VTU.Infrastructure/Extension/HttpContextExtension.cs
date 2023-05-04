using System.Security.Claims;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

namespace VTU.Infrastructure.Extension;

/// <summary>
/// HttpContext扩展类
/// </summary>
public static class HttpContextExtension
{
    /// <summary>
    /// 获取请求令牌
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static string GetToken(this HttpContext context)
    {
        return context.Request.Headers["Authorization"]!;
    }

    /// <summary>
    /// 获取客户端IP
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static string GetClientUserIp(this HttpContext? context)
    {
        if (context == null) return "";
        var result = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (string.IsNullOrEmpty(result))
        {
            result = context.Connection.RemoteIpAddress?.ToString();
        }

        if (string.IsNullOrEmpty(result) || result.Contains("::1"))
            result = "127.0.0.1";

        result = result.Replace("::ffff:", "127.0.0.1");
        result = IsIp(result) ? result : "127.0.0.1";
        return result;
    }

    public static bool IsIp(string ip)
    {
        return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
    }

    /// <summary>
    /// 获取登录用户id
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static long GetUId(this HttpContext context)
    {
        var uid = context.User.FindFirstValue(ClaimTypes.PrimarySid);
        return !string.IsNullOrEmpty(uid) ? long.Parse(uid) : 0;
    }
}