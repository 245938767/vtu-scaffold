using System.ComponentModel;
using VTU.Data.Models.Menus;
using VTU.Data.Models.Users;
using VTU.Infrastructure.Enums;

namespace VTU.Models.Response.Roles;

public class RoleResponse : Infrastructure.Models.Response
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
    public string RoleKey { get; set; }

    /// <summary>
    /// 角色排序
    /// </summary>
    [Description("角色排序")]
    public int RoleSort { get; set; }

    /// <summary>
    /// 角色状态
    /// </summary>
    [Description("角色状态")]
    public ValidStatus Status { get; set; }


    /// <summary>
    /// 数据范围（1：全部数据权限 2：自定数据权限 3：本部门数据权限 4：本部门及以下数据权限））
    /// </summary>
    [Description("数据范围（1：全部数据权限 2：自定数据权限 3：本部门数据权限 4：本部门及以下数据权限））")]
    public string DataScope { get; set; }

    /// <summary>
    /// 菜单树选择项是否关联显示
    /// </summary>
    [Description("菜单树选择项是否关联显示")]
    public bool MenuCheckStrictly { get; set; }

    /// <summary>
    /// 部门树选择项是否关联显示
    /// </summary>
    [Description("部门树选择项是否关联显示")]
    public bool DeptCheckStrictly { get; set; }

    /// <summary>
    /// 菜单组
    /// </summary>
    public virtual List<Menu> Menus { get; set; }

    public virtual List<User> Users { get; set; }
}