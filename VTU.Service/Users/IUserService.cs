using VTU.Infrastructure.Models;
using VTU.Models.Request.Users;

namespace VTU.Service.Users;

public interface IUserService
{
    public PagedInfo<Data.Models.Users.User> getUserList(UserQueryRequest userQueryRequest);

    public string login(LoginRequest loginRequest);
}