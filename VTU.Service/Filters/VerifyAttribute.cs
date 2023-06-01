using VTU.Infrastructure.Extension;
using VTU.Infrastructure.Models;
using VTU.Service.Helper;

namespace VTU.Service.Filters;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

/// <summary>
/// 授权校验访问
/// 如果跳过授权登录在Action 或controller加上 AllowAnonymousAttribute
/// </summary>
public class VerifyAttribute : Attribute, IAuthorizationFilter
{
    /// <summary>
    /// 只判断token是否正确，不判断权限
    /// </summary>
    /// <param name="context"></param>
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        //校验是否跳过
        var noNeedCheck = false;
        if (context.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
        {
            noNeedCheck = controllerActionDescriptor.MethodInfo.GetCustomAttributes(inherit: true)
                .Any(a => a.GetType() == typeof(AllowAnonymousAttribute));
        }

        if (noNeedCheck) return;

        var ip = context.HttpContext.GetClientUserIp();
        string url = context.HttpContext.Request.Path;
        // var isAuthed = context.HttpContext.User.Identity.IsAuthenticated;

        //使用jwt token校验
        var info = JwtHelper.GetLoginUser(context.HttpContext);

        if (info != null) return;
        var msg = $"请求访问[{url}]失败，无法访问系统资源";

        Console.WriteLine($"{msg}");


        ;
        JsonResult result = new(JsonObject<string>.Fail(url, msg))
        {
            ContentType = "application/json",
        };
        context.Result = result;
    }
}