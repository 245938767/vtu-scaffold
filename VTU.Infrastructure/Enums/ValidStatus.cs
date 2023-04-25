using System.ComponentModel;

namespace WebApplication1.Infrastructure.Enums;

[Flags]
public enum ValidStatus
{
    [Description("启用")] Valid = 1,
    [Description("禁用")] UnValid = 0,
}