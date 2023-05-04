using System.ComponentModel.DataAnnotations;

namespace VTU.Models.Request.Users;

public class CreateUserRequest
{
    [Required(ErrorMessage = "请输入用户名称")]
    [StringLength(20)]
    public string UserName { get; set; }

    [Required(ErrorMessage = "请输入用户密码")]
    [StringLength(20)]
    public string Password { get; set; }

    public List<long> RoleId { get; set; }
    public string? email { get; set; }

    public string phoneNumber { get; set; }
    public int sex { get; set; }
}