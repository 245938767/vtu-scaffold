using Microsoft.AspNetCore.Mvc;
using VTU.Infrastructure.Constant;
using VTU.Infrastructure.Exceptions;
using VTU.Infrastructure.Extension;
using VTU.Service.Helper;

namespace VTU.BaseApi.Controller;

[ApiController]
public class BaseController : ControllerBase
{
    /// <summary>
    /// 获取用户ID
    /// </summary>
    /// <returns>用户ID</returns>
    /// <exception cref="BusinessException"></exception>
    protected long CurrentUId()
    {
        var loginUserClaims = JwtHelper.GetLoginUserClaims(HttpContext);

        var value = loginUserClaims?.FirstOrDefault(x => x.Type == ClaimConstant.PrimarySid)?.Value;
        return !string.IsNullOrEmpty(value) ? long.Parse(value) : throw new BusinessException("获取用户信息失败");
    }

    /// <summary>
    /// 获得当前用户名称
    /// </summary>
    /// <returns>用户名称</returns>
    /// <exception cref="BusinessException"></exception>
    protected string CurrentUserName()
    {
        var loginUserClaims = JwtHelper.GetLoginUserClaims(HttpContext);
        var value = loginUserClaims?.FirstOrDefault(x => x.Type == ClaimConstant.Name)?.Value;
        if (value == null)
        {
            throw new BusinessException("获取用户名称失败");
        }

        return value;
    }

    /// <summary>
    /// 获得IP地址
    /// </summary>
    /// <returns></returns>
    protected string getIP()
    {
        return HttpContext.GetClientUserIp();
    }
}