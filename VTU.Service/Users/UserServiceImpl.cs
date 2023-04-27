using System.Reflection.Metadata;
using Mapster;
using Microsoft.EntityFrameworkCore;
using VTU.Data.Models;
using VTU.Data.Models.Users;
using VTU.Infrastructure.Attribute;
using VTU.Infrastructure.Constant;
using VTU.Infrastructure.Exceptions;
using VTU.Infrastructure.Extension;
using VTU.Infrastructure.Models;
using VTU.Models;
using VTU.Models.Request.Users;
using VTU.Models.Response;
using VTU.Service.Helper;

namespace VTU.Service.Users;

[AppService(ServiceType = typeof(IUserService), ServiceLifetime = LifeTime.Transient)]
public class UserServiceImpl : IUserService
{
    private readonly EntityDbContext _dbContext;

    public UserServiceImpl(EntityDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public PagedInfo<UserResponse> GetUserList(UserQueryRequest userQueryRequest)
    {
        var queryable = _dbContext.Users.AsQueryable().OrderByDescending(x => x.Id)
            .ToPage<User, UserResponse>(userQueryRequest);
        return queryable;
    }

    public string Login(LoginRequest loginRequest)
    {
        var user = _dbContext.Users.Include(x => x.Roles).FirstOrDefault(x =>
            x.UserName.Equals(loginRequest.UserName));
        if (user == null)
        {
            throw new BusinessException("用户不存在");
        }

        if (!user.Password.Equals(user.validatePassword(loginRequest.Password)))
        {
            throw new BusinessException("密码错误");
        }

        //检查用户是否已经登录
        var cache = CacheHelper.GetCache(GlobalConstant.UserPermKey + user.Id);
        if (cache != null)
        {
            throw new BusinessException("用户已登录");
        }

        var loginUserRoles = user.Roles.Select(x => x.Id);
        var roles = _dbContext.Roles.Include(x => x.Menus).Where(x => loginUserRoles.Contains(x.Id)).ToList();
        //获得pre的List数据
        var menuList = new List<string>();
        foreach (var loginUserRole in roles)
        {
            menuList.AddRange(loginUserRole.Menus.Select(x => x.Perms));
        }

        var loginUser = new LoginUser(user, user.Roles, menuList);
        //生成token
        return JwtHelper.GenerateJwtToken(JwtHelper.AddClaims(loginUser), loginUser);
    }

    public void LoginOut(long userId)
    {
        CacheHelper.Remove($"{GlobalConstant.UserPermKey}{userId}");
    }

    public UserResponse GetUserById(long id)
    {
        var firstOrDefault = _dbContext.Users.FirstOrDefault(x => x.Id == id);
        if (firstOrDefault == null)
        {
            throw new BusinessException("用户不存在");
        }

        return firstOrDefault.Adapt<UserResponse>();
    }
}