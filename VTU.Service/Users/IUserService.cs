using VTU.Infrastructure.Models;
using VTU.Models.Request.Users;
using VTU.Models.Response;

namespace VTU.Service.Users;

public interface IUserService
{
    public PagedInfo<UserResponse> getUserList(UserQueryRequest userQueryRequest);

    public string login(LoginRequest loginRequest);
}