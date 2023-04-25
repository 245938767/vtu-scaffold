using VTU.Data.Models.Roles;
using VTU.Data.Models.Users;

namespace VTU.Models;

/// <summary>
/// 登录用户信息存储
/// </summary>
public class LoginUser
{
    public long UserId { get; set; }
    public string UserName { get; set; }

    /// <summary>
    /// 角色集合
    /// </summary>
    public List<string> RoleIds { get; set; }

    /// <summary>
    /// 角色集合(数据权限过滤使用)
    /// </summary>
    public List<Role> Roles { get; set; }

    /// <summary>
    /// 权限集合
    /// </summary>
    public List<string> Permissions { get; set; } = new List<string>();

    private  LoginUser()
    {
    }

    public LoginUser(User user, List<Role> roles, List<string> permissions)
    {
        UserId = user.Id;
        UserName = user.UserName;
        Roles = roles ?? throw new ArgumentNullException(nameof(roles));
        RoleIds = roles.Select(f => f.RoleKey).ToList();
        Permissions = permissions;
    }
}