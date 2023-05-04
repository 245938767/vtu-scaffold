using Mapster;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using VTU.Data.Models;
using VTU.Data.Models.Users;
using VTU.Infrastructure.Attribute;
using VTU.Infrastructure.Constant;
using VTU.Infrastructure.Enums;
using VTU.Infrastructure.Exceptions;
using VTU.Infrastructure.Extension;
using VTU.Infrastructure.Models;
using VTU.Models.Request.Users;
using VTU.Models.Response;
using VTU.Service.Helper;

namespace VTU.Service.Users;

[Service(ServiceType = typeof(IUserService), ServiceLifetime = LifeTime.Transient)]
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

    public string Login(LoginRequest loginRequest, string? ip)
    {
        var user = _dbContext.Users.Include(x => x.Roles).FirstOrDefault(x =>
            x.UserName.Equals(loginRequest.UserName));
        if (user == null)
        {
            throw new BusinessException("用户不存在");
        }

        //校验用户密码信息并设置
        user.ValidUser(loginRequest.Password, ip);

        //检查用户是否已经登录
        var cache = CacheHelper.GetCache(GlobalConstant.UserPermKey + user.Id);
        if (cache != null)
        {
            throw new BusinessException("用户已登录");
        }

        var loginUserRoles = user.Roles.Select(x => x.Id);
        //初始化菜单
        var menuList = (from userRole in user.Roles where userRole.IsAdmin() select GlobalConstant.AdminPerm).ToList();
        var roles = _dbContext.Roles.Include(x => x.Menus).Where(x => loginUserRoles.Contains(x.Id)).ToList();
        //获得menu的List数据
        foreach (var loginUserRole in roles)
        {
            menuList.AddRange(loginUserRole.Menus.Select(x => x.Perms));
        }

        var loginUser = new LoginUser(user, user.Roles, menuList);
        //生成token
        var generateJwtToken = JwtHelper.GenerateJwtToken(JwtHelper.AddClaims(loginUser), loginUser);
        //保存用户登录时的修改信息（ip/登录时间）
        _dbContext.Update(user);
        _dbContext.SaveChanges();
        return generateJwtToken;
    }

    public void LoginOut(long userId)
    {
        CacheHelper.Remove($"{GlobalConstant.UserPermKey}{userId}");
    }

    public void register(RegisterUserRequest registerUserRequest)
    {
        ValidUserName(registerUserRequest.UserName, "用户已注册");

        //获得role
        var queryable = _dbContext.Roles.Where(x => registerUserRequest.RoleId == x.Id).ToList();
        //创建用户基础信息
        var user = new User();
        user.create(registerUserRequest.UserName, registerUserRequest.Password, registerUserRequest.email,
            registerUserRequest.phoneNumber, registerUserRequest.sex, queryable);
        _dbContext.Users.Add(user);
        var x = _dbContext.SaveChanges();
        if (x <= 0)
        {
            throw new BusinessException("用户注册失败");
        }
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

    public void CreateUser(CreateUserRequest createUserRequest)
    {
        ValidUserName(createUserRequest.UserName);
        //获得role
        var queryable = _dbContext.Roles.Where(x => createUserRequest.RoleId.Contains(x.Id)).ToList();
        //创建用户基础信息
        var user = new User();
        user.create(createUserRequest.UserName, createUserRequest.Password, createUserRequest.email,
            createUserRequest.phoneNumber, createUserRequest.sex, queryable);
        _dbContext.Users.Add(user);
        var x = _dbContext.SaveChanges();
        if (x <= 0)
        {
            throw new BusinessException("用户创建失败");
        }
    }

    public void EditUser(EditUserRequest editUserRequest)
    {
        var editUser = _dbContext.Users.FirstOrDefault(x => x.Id == editUserRequest.Id);
        if (editUser == null)
        {
            throw new BusinessException("未找到当前用户");
        }

        //edit
        var asQueryable = _dbContext.Users.Where(x => x.Id != editUserRequest.Id).AsQueryable();
        if (editUserRequest.UserName != null)
        {
            editUser.UserName = editUserRequest.UserName;
            asQueryable = asQueryable.Where(x => x.UserName == editUserRequest.UserName);
        }

        if (editUserRequest.Phonenumber != null)
        {
            editUser.Phonenumber = editUserRequest.Phonenumber;
            // asQueryable=asQueryable.Where()
        }

        if (editUserRequest.Email != null)
        {
            editUser.Email = editUserRequest.Email;
            asQueryable = asQueryable.Where(x => x.Email == editUserRequest.Email);
        }

        if (editUserRequest.Gender != null)
        {
            editUser.Gender = (Gender)Enum.ToObject(typeof(Gender), editUserRequest.Gender);
        }

        if (editUserRequest.NickName != null)
        {
            editUser.NickName = editUserRequest.NickName;
        }

        if (asQueryable.Any())
        {
            throw new BusinessException("用户信息校验失败");
        }

        _dbContext.Update(editUser);
        var saveChanges = _dbContext.SaveChanges();
        if (saveChanges <= 0)
        {
            throw new BusinessException("修改失败");
        }
    }

    public void DeleteUser(long userId)
    {
        var firstOrDefaultUser = _dbContext.Users.Include(x => x.Roles).FirstOrDefault(x => x.Id == userId);
        if (firstOrDefaultUser == null)
        {
            throw new BusinessException("用户不存在");
        }

        _dbContext.Remove(firstOrDefaultUser);
        _dbContext.SaveChanges();
    }

    private void ValidUserName(string userName, string? errorMessage = "用户已经存在")
    {
        var where = _dbContext.Users.FirstOrDefault(x => x.UserName == userName);
        if (where != null)
        {
            throw new BusinessException(errorMessage);
        }
    }
}