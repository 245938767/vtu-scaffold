using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using VTU.Data.Models;
using VTU.Data.Models.Roles;
using VTU.Infrastructure.Attribute;
using VTU.Infrastructure.Exceptions;
using VTU.Infrastructure.Extension;
using VTU.Infrastructure.Models;
using VTU.Models.Request.Roles;
using VTU.Models.Response.Roles;

namespace VTU.Service.Roles;

[Service(ServiceType = typeof(IRoleService), ServiceLifetime = LifeTime.Transient)]
public class RoleServiceImpl : IRoleService
{
    private readonly EntityDbContext _dbContext;

    public RoleServiceImpl(EntityDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public PagedInfo<RoleResponse> SelectRoleList(RoleQueryRequest roleQueryRequest)
    {
        var queryable = _dbContext.Roles.AsQueryable().OrderByDescending(x => x.Id)
            .ToPage<Role, RoleResponse>(roleQueryRequest);
        return queryable;
    }

    public List<RoleResponse> SelectRoleAll()
    {
        return _dbContext.Roles.ToList().Adapt<List<RoleResponse>>();
    }


    public RoleResponse SelectRoleById(long roleId)
    {
        var firstOrDefault = _dbContext.Roles.Include(x => x.Menus).FirstOrDefault(x => x.Id == roleId);
        if (firstOrDefault == null)
        {
            throw new BusinessException("未找到此角色");
        }

        return firstOrDefault.Adapt<RoleResponse>();
    }

    public int DeleteRoleByRoleId(long[] roleIds)
    {
        var roles = _dbContext.Roles.Include(x => x.Users).Where(x => roleIds.Contains(x.Id)).ToList();
        _dbContext.RemoveRange(roles);
        return _dbContext.SaveChanges();
    }

    public int UpdateRoleStatus(Role roleDto)
    {
        throw new NotImplementedException();
    }

    public void CheckRoleAllowed(Role role)
    {
        throw new NotImplementedException();
    }

    public long InsertRole(CreateRoleRequest createRoleRequest)
    {
        var queryable = _dbContext.Roles.Any(x => x.RoleName == createRoleRequest.RoleName);
        if (queryable)
        {
            throw new BusinessException("此名称已存在");
        }

        var role = new Role().create(createRoleRequest.RoleName, createRoleRequest.RoleKey, createRoleRequest.RoleSort,
            createRoleRequest.DataScope);
        if (!createRoleRequest.menuList.IsNullOrEmpty())
        {
            role.updateMenu(_dbContext.Menus.Where(x => createRoleRequest.menuList.Contains(x.Id)).ToList());
        }

        _dbContext.Add(role);
        _dbContext.SaveChanges();
        return role.Id;
    }


    public List<long> SelectUserRoleMenus(long roleId)
    {
        throw new NotImplementedException();
    }

    public List<long> SelectRoleMenuByRoleIds(long[] roleIds)
    {
        throw new NotImplementedException();
    }


    public List<long> SelectUserRoles(long userId)
    {
        throw new NotImplementedException();
    }

    public List<string> SelectUserRoleKeys(long userId)
    {
        throw new NotImplementedException();
    }

    public List<string> SelectUserRoleNames(long userId)
    {
        throw new NotImplementedException();
    }

    public int UpdateRole(EditRoleRequest editRoleRequest)
    {
        #region valid

        var asQueryable = _dbContext.Roles.Where(x => x.Id != editRoleRequest.Id).AsQueryable();
        if (editRoleRequest.RoleName != null)
        {
            asQueryable = asQueryable.Where(x => x.RoleName == editRoleRequest.RoleName);
        }

        if (asQueryable.Any())
        {
            throw new BusinessException("名称已存在");
        }

        #endregion

        var firstOrDefault = _dbContext.Roles.FirstOrDefault(x => x.Id == editRoleRequest.Id);
        if (firstOrDefault == null)
        {
            throw new BusinessException("未找到此角色");
        }

        firstOrDefault.update(editRoleRequest.RoleName, editRoleRequest.RoleKey, editRoleRequest.RoleSort,
            editRoleRequest.DataScope);
        if (!editRoleRequest.menuList.IsNullOrEmpty())
        {
            firstOrDefault.updateMenu(_dbContext.Menus.Where(x => editRoleRequest.menuList.Contains(x.Id)).ToList());
        }

        _dbContext.Update(firstOrDefault);
        return _dbContext.SaveChanges();
    }
}