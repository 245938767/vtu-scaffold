using Microsoft.AspNetCore.Mvc;
using VTU.BaseApi.Controller;
using VTU.Infrastructure.Models;
using VTU.Models.Request.Users;
using VTU.Models.Response;
using VTU.Service.Filters;
using VTU.Service.Users;

namespace VTU.WebApi.Controllers;

[Verify]
[Route("[controller]/v1")]
public class UserController : BaseController
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// 获取用户信息
    /// </summary>
    /// <param name="userQueryRequest"></param>
    /// <returns></returns>
    [HttpGet("/userInfoList")]
    [PermissionFilter("user:list")]
    public JsonObject<PagedInfo<UserResponse>> GetUserinfoList([FromQuery] UserQueryRequest userQueryRequest)
    {
        return _userService.GetUserList(userQueryRequest);
    }

    /// <summary>
    /// 获得用户信息
    /// </summary>
    /// <returns></returns>
    [HttpGet("/userInfo/{userId:long}")]
    [PermissionFilter("user:info")]
    public JsonObject<UserResponse> GetUserInfo(long userId)
    {
        return _userService.GetUserById(userId);
    }

    /// <summary>
    /// 创建用户
    /// </summary>
    /// <param name="createUserRequest"></param>
    /// <returns></returns>
    [PermissionFilter("user:create")]
    [HttpPost("/create")]
    public JsonObject<string> CreateUser([FromBody] CreateUserRequest createUserRequest)
    {
        _userService.CreateUser(createUserRequest);
        return "";
    }

    /// <summary>
    /// 修改用户
    /// </summary>
    /// <param name="editUserRequest"></param>
    /// <returns></returns>
    [PermissionFilter("user:edit")]
    [HttpPost("/edit")]
    public JsonObject<string> EditUser([FromBody] EditUserRequest editUserRequest)
    {
        _userService.EditUser(editUserRequest);
        return "";
    }

    /// <summary>
    /// 删除用户
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [PermissionFilter("user:delete")]
    [HttpPost("/delete/{userId:long}")]
    public JsonObject<string> DeleteUser(long userId)
    {
        _userService.DeleteUser(userId);
        return "";
    }
}