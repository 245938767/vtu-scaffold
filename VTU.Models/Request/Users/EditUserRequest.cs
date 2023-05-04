using System.ComponentModel;

namespace VTU.Models.Request.Users;

public class EditUserRequest
{
    [Description("ID")] public int Id { get; init; }

    public string? UserName { get; set; }

    public string? NickName { get; set; }


    public string? Email { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    [Description("手机号")]
    public string? Phonenumber { get; set; }

    /// <summary>
    /// 用户性别（0男 1女 2未知）
    /// </summary>
    [Description("用户性别（0男 1女 2未知）")]
    public int? Gender { get; set; }
}