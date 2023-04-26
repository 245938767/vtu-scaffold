using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VTU.BaseApi.Controller;
using VTU.Data.Models;
using VTU.Data.Models.Users;
using VTU.Infrastructure.Constant;
using VTU.Infrastructure.Models;
using VTU.Models;
using VTU.Service.Helper;
using VTU.WebApi.Request;

namespace VTU.WebApi.Controllers;

[Route("[controller]/v1")]
public class LoginController : BaseController
{
    private readonly EntityDbContext _dbContext;

    public LoginController(EntityDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpPost("/login")]
    public JsonObject<string> LoginUser([FromBody] LoginUserInfoRequest loginUserInfoRequest)
    {
        var loginUser = _dbContext.Users.Include(x => x.Roles).FirstOrDefault(x =>
            x.UserName.Equals(loginUserInfoRequest.UserName));
        if (loginUser == null)
        {
            return JsonObject<string>.Fail("", "用户不存在");
        }

        if (!loginUser.Password.Equals(loginUser.validatePassword(loginUserInfoRequest.Password)))
        {
            return JsonObject<string>.Fail("", "密码错误");
        }

        var loginUserRoles = loginUser.Roles.Select(x => x.Id);
        var roles = _dbContext.Roles.Include(x => x.Menus).Where(x => loginUserRoles.Contains(x.Id)).ToList();
        //获得pre的List数据
        var menuList = new List<string>();
        foreach (var loginUserRole in roles)
        {
            menuList.AddRange(loginUserRole.Menus.Select(x => x.Perms));
        }

        var user = new LoginUser(loginUser, loginUser.Roles, menuList);
        //设置菜单数据
        CacheHelper.SetCache(GlobalConstant.UserPermKey + user.UserId, menuList);
        //生成token
        var generateJwtToken = JwtHelper.GenerateJwtToken(JwtHelper.AddClaims(user));


        return generateJwtToken;
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