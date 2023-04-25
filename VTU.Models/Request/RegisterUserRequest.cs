namespace WebApplication1.Request;

public class RegisterUserRequest
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public List<long> RoleId { get; set; }
    public string? email { get; set; }

    public string phoneNumber { get; set; }
    public string sex { get; set; }
}