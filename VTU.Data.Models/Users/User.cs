using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using VTU.Data.Models.Roles;
using VTU.Infrastructure.Enums;
using VTU.Infrastructure.Helper;

namespace VTU.Data.Models.Users;

/// <summary>
/// 用户表
/// </summary>
///
[Description("用户表")]
[Table("user")]
public class User : BaseEntity
{
    public string UserName { get; set; }
    public string NickName { get; set; }


    public string? Email { get; set; }

    [Description("Password")] public string Password { get; set; }

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

    [Description("密码加密的盐")] public string Salt { get; set; }


    /// <summary>
    /// 帐号状态
    /// </summary>
    [Description("帐号状态")]
    public ValidStatus Status { get; set; } = ValidStatus.Valid;

    /// <summary>
    /// 删除标志
    /// </summary>
    [Description("删除标志")]
    public ValidStatus DelFlag { get; set; } = ValidStatus.UnValid;

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


    [JsonIgnore] public virtual List<Role> Roles { get; set; }


    #region 其他方法

    public bool IsAdmin()
    {
        return IsAdmin(Id);
    }

    public static bool IsAdmin(long userId)
    {
        return 1 == userId;
    }

    /// <summary>
    /// 用户信息初始化方法
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="password"></param>
    /// <param name="email"></param>
    /// <param name="phoneNumber"></param>
    /// <param name="gender"></param>
    /// <param name="roles"></param>
    public User create(string userName, string password, string? email, string phoneNumber, string gender,
        List<Role> roles)
    {
        this.NickName = userName;
        this.UserName = userName;
        this.Phonenumber = phoneNumber;
        this.Email = email;
        this.Gender = gender;
        this.Roles = roles;
        preCreateTime();
        setPassword(password);
        return this;
    }

    public void setPassword(string password)
    {
        this.Salt = PasswordHelper.GenerateSalt();
        this.Password = PasswordHelper.EncryptPassword(password, this.Salt);
    }

    public string validatePassword(string password)
    {
        return PasswordHelper.EncryptPassword(password, this.Salt);
    }

    #endregion
}