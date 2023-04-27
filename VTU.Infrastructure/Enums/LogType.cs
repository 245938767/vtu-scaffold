using System.ComponentModel;

namespace VTU.Infrastructure.Enums;

public enum LogType
{
    /// <summary>
    /// 其它
    /// </summary>
    [Description("其他")] OTHER = 0,

    /// <summary>
    /// 新增
    /// </summary>
    [Description("新增")] INSERT = 1,

    /// <summary>
    /// 修改
    /// </summary>
    [Description("修改")] UPDATE = 2,

    /// <summary>
    /// 删除
    /// </summary>
    [Description("删除")] DELETE = 3,

    /// <summary>
    /// 授权
    /// </summary>
    [Description("授权")] GRANT = 4,

    /// <summary>
    /// 导出
    /// </summary>
    [Description("导出")] EXPORT = 5,

    /// <summary>
    /// 导入
    /// </summary>
    [Description("导入")] IMPORT = 6,

    /// <summary>
    /// 强退
    /// </summary>
    [Description("强退")] FORCE = 7,


    /// <summary>
    /// 清空数据
    /// </summary>
    [Description("清空数据")] CLEAN = 8,
}