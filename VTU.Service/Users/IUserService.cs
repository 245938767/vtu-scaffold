using VTU.Infrastructure.Models;
using VTU.Models.Request.Users;
using VTU.Models.Response;
using VTU.Models.Response.Users;

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
    /// <param name="ip"></param>
    /// <returns></returns>
    public string Login(LoginRequest loginRequest, string? ip);

    /// <summary>
    /// 用户退出登录
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public void LoginOut(long userId);

    /// <summary>
    /// 用户注册方法
    /// </summary>
    /// <param name="registerUserRequest"></param>
    public void register(RegisterUserRequest registerUserRequest);

    /// <summary>
    /// 获得用户个人信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public UserResponse GetUserById(long id);

    /// <summary>
    /// 创建用户
    /// </summary>
    /// <param name="createUserRequest"></param>
    public void CreateUser(CreateUserRequest createUserRequest);


    /// <summary>
    /// 修改用户信息
    /// </summary>
    /// <param name="editUserRequest"></param>
    public void EditUser(EditUserRequest editUserRequest);

    /// <summary>
    /// 删除用户ID
    /// </summary>
    /// <param name="userId"></param>
    public void DeleteUser(long userId);
}