using System.ComponentModel;
using Microsoft.AspNetCore.Http;

namespace VTU.Infrastructure.Enums;

public enum CodeEnums
{
    [Description("操作成功")] Success = StatusCodes.Status200OK,
    [Description("操作失败")] Fail = StatusCodes.Status500InternalServerError,
    [Description("未查询到信息")] NotFindError = 1001,
    [Description("保存信息失败")] SaveError = 1002,
    [Description("更新信息失败")] UpdateError = 1003,
    [Description("数据检验失败")] ValidateError = 1004,
    [Description("状态已经被启用")] StatusHasValid = 1005,
    [Description("状态已经被禁用")] StatusHasInvalid = 1006,
    [Description("系统异常")] SystemError = 1007,
    [Description("业务异常")] BusinessError = 1008,
}