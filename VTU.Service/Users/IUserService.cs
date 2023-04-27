using VTU.Infrastructure.Models;
using VTU.Models.Request.Users;
using VTU.Models.Response;

namespace VTU.Service.Users;

public interface IUserService
{
    /// <summary>
    /// 用户列表
    /// </summary>
    /// <param name="userQueryRequest"></param>
    /// <returns></returns>
    public PagedInfo<UserResponse> GetUserList(UserQueryRequest userQueryRequest);

    /// <summary>
    /// 登录
    /// </summary>
    /// <param name="loginRequest"></param>
    /// <returns></returns>
    public string Login(LoginRequest loginRequest);

    /// <summary>
    /// 用户退出登录
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public void LoginOut(long userId);

    /// <summary>
    /// 获得用户个人信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public UserResponse GetUserById(long id);
}