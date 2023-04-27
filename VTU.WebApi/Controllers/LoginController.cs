using Microsoft.AspNetCore.Mvc;
using VTU.BaseApi.Controller;
using VTU.Data.Models;
using VTU.Data.Models.Users;
using VTU.Infrastructure.Models;
using VTU.Models.Request.Users;
using VTU.Service.Users;
using VTU.WebApi.Request;

namespace VTU.WebApi.Controllers;

[Route("[controller]/v1")]
public class LoginController : BaseController
{
    private readonly IUserService _userService;
    private readonly EntityDbContext _dbContext;

    public LoginController(IUserService userService, EntityDbContext dbContext)
    {
        _userService = userService;
        _dbContext = dbContext;
    }

    [HttpPost("/login")]
    public JsonObject<string> LoginUser([FromBody] LoginRequest loginRequest)
    {
        return _userService.Login(loginRequest);
    }

    [HttpPost("/loginOut")]
    public JsonObject<string> LoginOut(long userId)
    {
        _userService.LoginOut(userId);
        return "退出登录成功";
    }

    [HttpPost("/register")]
    public JsonObject<string> CreateUser([FromBody] RegisterUserRequest registerUserRequest)
    {
        //获得role
        var queryable = _dbContext.Roles.Where(x => registerUserRequest.RoleId.Contains(x.Id)).ToList();
        //创建用户基础信息
        var user = new User();
        user.create(registerUserRequest.UserName, registerUserRequest.Password, registerUserRequest.email,
            registerUserRequest.phoneNumber, registerUserRequest.sex, queryable);
        _dbContext.Users.Add(user);
        _dbContext.SaveChanges();
        return "";
    }
}