using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using VTU.Models;
using VTU.Service.Helper;
using WebApplication1.Infrastructure.Constant;

namespace VTU.Service.Filters
{
    /// <summary>
    /// API授权判断
    /// </summary>
    public class PermissionFilter : ActionFilterAttribute
    {
        /// <summary>
        /// 权限字符串
        /// </summary>
        public string Permission { get; } = null!;

        /// <summary>
        /// 是否有权限
        /// </summary>
        private bool HasRole { get; set; }

        private PermissionFilter()
        {
        }

        public PermissionFilter(string permission)
        {
            Permission = permission;
            HasRole = !string.IsNullOrEmpty(Permission);
        }

        /// <summary>
        /// 校验是否有权限访问
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var url = context.HttpContext.Request.Path;

            LoginUser? info = JwtHelper.GetLoginUser(context.HttpContext);
            //检查信息
            if (info == null)
            {
                JsonResult results = new(new
                {
                    Code = 500,
                    Msg = $"你当前没有权限访问或者已过期，请先登录",
                    Data = url
                })
                {
                    ContentType = "application/json",
                };
                context.Result = results;

                return base.OnActionExecutionAsync(context, next);
            }

            var perms = info.Permissions;
            var rolePerms = info.RoleIds;
            //判断是否超管
            if (perms.Exists(f => f.Equals(GlobalConstant.AdminPerm)))
            {
                HasRole = true;
            }
            else if (rolePerms.Exists(f => f.Equals(GlobalConstant.AdminRole)))
            {
                HasRole = true;
            }
            //当前属性标注是否拥有权限
            else if (!string.IsNullOrEmpty(Permission))
            {
                HasRole = perms.Exists(f => string.Equals(f, Permission, StringComparison.CurrentCultureIgnoreCase));
            }


            if (HasRole || Permission.Equals("common")) return base.OnActionExecutionAsync(context, next);
            Console.WriteLine($"用户{info.UserName}没有权限访问{url}，当前权限[{Permission}]");
            JsonResult result = new(new
            {
                Code = 500,
                Msg = $"你当前没有权限[{Permission}]访问,请联系管理员",
                Data = url
            })
            {
                ContentType = "application/json",
            };
            context.Result = result;

            return base.OnActionExecutionAsync(context, next);
        }
    }
}