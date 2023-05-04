using System.ComponentModel;

namespace VTU.Infrastructure.Enums;

public enum Gender
{
    [Description("男")] Male = 0,
    [Description("女")] Female = 1,
    [Description("未知")] Unknown = 2,
}