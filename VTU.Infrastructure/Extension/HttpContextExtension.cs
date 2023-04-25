
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
}