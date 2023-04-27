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

    public PagedInfo<User> getUserList(UserQueryRequest userQueryRequest)
    {
        var queryable = _dbContext.Users.AsQueryable().OrderByDescending(x => x.Id).ToPage(userQueryRequest);
        return queryable;
    }

    public string login(LoginRequest loginRequest)
    {
        var loginUser = _dbContext.Users.Include(x => x.Roles).FirstOrDefault(x =>
            x.UserName.Equals(loginRequest.UserName));
        if (loginUser == null)
        {
            throw new BusinessException("用户不存在");
        }

        if (!loginUser.Password.Equals(loginUser.validatePassword(loginRequest.Password)))
        {
            throw new BusinessException("密码错误");
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
        return JwtHelper.GenerateJwtToken(JwtHelper.AddClaims(user));
    }
}