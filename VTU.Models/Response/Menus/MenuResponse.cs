using System.ComponentModel;
using VTU.Data.Models.Menus;
using VTU.Data.Models.Roles;
using VTU.Infrastructure.Enums;

namespace VTU.Models.Response.Menus;

public class MenuResponse : Infrastructure.Models.Response
{
    /// <summary>
    /// 菜单名称
    /// </summary>
    [Description("菜单名称")]
    public string MenuName { get; set; }

    /// <summary>
    /// 父菜单ID
    /// </summary>
    [Description("父Id")]
    public int ParentId { get; set; } = 0;


    /// <summary>
    /// 显示顺序
    /// </summary>
    [Description("显示顺序")]
    public int OrderNum { get; set; }

    /// <summary>
    /// 路由地址
    /// </summary>
    [Description("路由地址")]
    public string Path { get; set; } = "#";

    /// <summary>
    /// 组件路径
    /// </summary>
    [Description("组件路径")]
    public string Component { get; set; }

    /// <summary>
    /// 是否缓存（1缓存 0不缓存）
    /// </summary>
    [Description("是否缓存（1缓存 0不缓存）")]
    public string IsCache { get; set; }

    /// <summary>
    /// 是否外链 
    /// </summary>
    [Description("是否外链")]
    public ValidStatus IsFrame { get; set; }

    /// <summary>
    /// 类型（M目录 C菜单 F按钮 L链接）
    /// </summary>
    [Description("类型（M目录 C菜单 F按钮 L链接）")]
    public string MenuType { get; set; }

    /// <summary>
    /// 显示状态
    /// </summary>
    [Description("显示状态")]
    public ValidStatus Visible { get; set; }

    /// <summary>
    /// 菜单状态
    /// </summary>
    [Description("菜单状态")]
    public ValidStatus Status { get; set; }

    /// <summary>
    /// 权限字符串
    /// </summary>
    [Description("权限字符串")]
    public string Perms { get; set; }

    /// <summary>
    /// 菜单图标
    /// </summary>
    [Description("菜单图标")]
    public string Icon { get; set; } = string.Empty;

    /// <summary>
    /// 菜单名key
    /// </summary>
    [Description("菜单名key")]
    public string MenuNameKey { get; set; }

    /// <summary>
    /// 子菜单
    /// </summary>
    ///
    public virtual ICollection<Menu> Children { get; set; } = new List<Menu>();

    // public virtual Menu Parent { get; set; }

    /// <summary>
    /// 子菜单个数
    /// </summary>
    public virtual int SubNum { get; set; }

    /// <summary>
    /// 是否包含子节点，前端用
    /// </summary>
    public virtual bool HasChildren => SubNum > 0 || Children.Count > 0;

    public virtual List<Role> Roles { get; set; }
}