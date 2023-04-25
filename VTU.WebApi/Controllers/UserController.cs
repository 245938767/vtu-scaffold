using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VTU.Data.Models;
using VTU.Data.Models.Users;
using VTU.Models;
using VTU.Service.Helper;
using WebApplication1.Infrastructure.Constant;
using WebApplication1.Request;

namespace VTU.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly EntityDbContext _dbContext;

    public UserController(EntityDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("userInfo")]
    // [PermissionFilter("user:list")]
    public ActionResult<List<User>> getUserinfo(int page = 1, int pageSize = 20)
    {
        var queryable = _dbContext.Users.AsQueryable().OrderByDescending(x => x.Id).Skip((page - 1) * pageSize)
            .Take(pageSize);
        return Ok(queryable);
    }

    [HttpPost("/login")]
    public ActionResult LoginUser([FromBody] LoginUserInfoRequest loginUserInfoRequest)
    {
        var loginUser = _dbContext.Users.Include(x => x.Roles).FirstOrDefault(x =>
            x.UserName.Equals(loginUserInfoRequest.UserName));
        if (loginUser == null)
        {
            return BadRequest("用户不存在");
        }

        if (!loginUser.Password.Equals(loginUser.validatePassword(loginUserInfoRequest.Password)))
        {
            return BadRequest("密码错误");
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


        return Ok(generateJwtToken);
    }

    [HttpPost("/register")]
    public ActionResult CreateUser([FromBody] RegisterUserRequest registerUserRequest)
    {
        //获得role
        var queryable = _dbContext.Roles.Where(x => registerUserRequest.RoleId.Contains(x.Id)).ToList();
        //创建用户基础信息
        var user = new User();
        user.create(registerUserRequest.UserName, registerUserRequest.Password, registerUserRequest.email,
            registerUserRequest.phoneNumber, registerUserRequest.sex, queryable);
        _dbContext.Users.Add(user);
        _dbContext.SaveChanges();
        return Ok();
    }
}