using Microsoft.AspNetCore.Mvc;
using VTU.BaseApi.Controller;
using VTU.Infrastructure.Models;
using VTU.Models.Request.Users;
using VTU.Models.Response;
using VTU.Service.Users;

namespace VTU.WebApi.Controllers;

[ApiController]
[Route("[controller]/v1")]
public class UserController : BaseController
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("/userInfoList")]
    // [PermissionFilter("user:list")]
    public JsonObject<PagedInfo<UserResponse>> GetUserinfoList([FromQuery] UserQueryRequest userQueryRequest)
    {
        return _userService.GetUserList(userQueryRequest);
    }

    [HttpGet("/userInfo")]
    public JsonObject<UserResponse> getUserInfo()
    {
        return _userService.GetUserById(CurrentUId());
    }
}