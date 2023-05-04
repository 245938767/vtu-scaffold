using System.ComponentModel;
using VTU.Data.Models.Roles;
using VTU.Infrastructure.Enums;

namespace VTU.Models.Response.Users;

public class UserResponse : Infrastructure.Models.Response
{
    public string UserName { get; set; }
    public string NickName { get; set; }

    public string? Email { get; set; }


    /// <summary>
    /// 手机号
    /// </summary>
    [Description("手机号")]
    public string Phonenumber { get; set; }

    /// <summary>
    /// 用户性别（0男 1女 2未知）
    /// </summary>
    [Description("用户性别（0男 1女 2未知）")]
    public string Gender { get; set; }


    /// <summary>
    /// 帐号状态
    /// </summary>
    [Description("帐号状态")]
    public ValidStatus Status { get; set; } = ValidStatus.Valid;

    /// <summary>
    /// 最后登录IP
    /// </summary>
    [Description("最后登录IP")]
    public string? LoginIP { get; set; }

    /// <summary>
    /// 最后登录时间
    /// </summary>
    [Description("最后登录时间")]
    public DateTime? LoginDate { get; set; }


    public virtual List<Role> Roles { get; set; }
}