using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VTU.BaseApi.Controller;
using VTU.Infrastructure.Models;
using VTU.Models.Request.Users;
using VTU.Service.Filters;
using VTU.Service.Users;

namespace VTU.WebApi.Controllers;

[Route("[controller]/v1")]
[Verify]
public class LoginController : BaseController
{
    private readonly IUserService _userService;

    public LoginController(IUserService userService)
    {
        _userService = userService;
    }

    [AllowAnonymous]
    [HttpPost("/login")]
    public JsonObject<string> LoginUser([FromBody] LoginRequest loginRequest)
    {
        return _userService.Login(loginRequest, getIP());
    }

    [HttpPost("/loginOut")]
    public JsonObject<string> LoginOut()
    {
        _userService.LoginOut(CurrentUId());
        return "退出登录成功";
    }

    [AllowAnonymous]
    [HttpPost("/register")]
    public JsonObject<string> CreateUser([FromBody] RegisterUserRequest registerUserRequest)
    {
        _userService.register(registerUserRequest);
        return "";
    }
}