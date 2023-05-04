using System.ComponentModel;

namespace VTU.Models.Request.Roles;

public class CreateRoleRequest
{

    /// <summary>
    /// 角色名称
    /// </summary>
    [Description("角色名称")]
    public string RoleName { get; set; }

    /// <summary>
    /// 角色权限
    /// </summary>
    [Description("角色权限")]
    public string? RoleKey { get; set; }

    /// <summary>
    /// 角色排序
    /// </summary>
    [Description("角色排序")]
    public int? RoleSort { get; set; }

    public string? DataScope { get; set; }

    [Description("绑定菜单")] public List<long>? menuList { get; set; }
}