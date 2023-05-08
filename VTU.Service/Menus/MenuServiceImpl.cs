using Mapster;
using Microsoft.EntityFrameworkCore;
using VTU.Data.Models;
using VTU.Data.Models.Menus;
using VTU.Infrastructure.Attribute;
using VTU.Infrastructure.Exceptions;
using VTU.Models.Request.Menus;
using VTU.Models.Response.Menus;

namespace VTU.Service.Menus;

[Service(ServiceType = typeof(IMenuService), ServiceLifetime = LifeTime.Transient)]
public class MenuServiceImpl : IMenuService
{
    private readonly EntityDbContext _dbContext;

    public MenuServiceImpl(EntityDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public List<MenuResponse> SelectTreeMenuList()
    {
        var menus = _dbContext.Menus.ToList();
        return TranslationTree(menus.Adapt<List<MenuResponse>>());
    }

    public MenuResponse GetMenuByMenuId(int menuId)
    {
        var firstOrDefault = CheckMenuNullable(_dbContext.Menus.FirstOrDefault(x => x.Id == menuId));
        return firstOrDefault.Adapt<MenuResponse>();
    }

    public List<MenuResponse> GetMenusByMenuId(int menuId, int userId)
    {
        throw new NotImplementedException();
    }

    public int AddMenu(CreateMenuRequest menu)
    {
        var menu1 = menu.ToMenu();
        _dbContext.Menus.Add(menu1);
        return _dbContext.SaveChanges();
    }

    public int EditMenu(EditMenuRequest menu)
    {
        //check
        if (CheckMenuNameUnique(menu.Id, menu.MenuName))
        {
            throw new BusinessException("名称已存在");
        }

        var firstOrDefault = _dbContext.Menus.FirstOrDefault(x => x.Id == menu.Id);
        firstOrDefault = CheckMenuNullable(firstOrDefault);
        firstOrDefault.MenuName = menu.MenuName;
        firstOrDefault.Component = menu.Component ?? firstOrDefault.Component;
        firstOrDefault.Status = menu.Status ?? firstOrDefault.Status;
        firstOrDefault.Visible = menu.Visible ?? firstOrDefault.Visible;
        firstOrDefault.Path = menu.Path ?? firstOrDefault.Path;
        firstOrDefault.IsFrame = menu.IsFrame ?? firstOrDefault.IsFrame;
        firstOrDefault.Icon = menu.Icon ?? firstOrDefault.Icon;
        firstOrDefault.Perms = menu.Perms ?? firstOrDefault.Perms;
        firstOrDefault.MenuType = menu.MenuType ?? firstOrDefault.MenuType;
        firstOrDefault.IsCache = menu.IsCache ?? firstOrDefault.IsCache;
        firstOrDefault.MenuNameKey = menu.MenuNameKey ?? firstOrDefault.MenuNameKey;
        _dbContext.Update(firstOrDefault);
        return _dbContext.SaveChanges();
    }

    public void DeleteMenuById(int menuId)
    {
        var singleOrDefault = _dbContext.Menus.Include(x => x.Roles).SingleOrDefault(x => x.Id == menuId);
        singleOrDefault = CheckMenuNullable(singleOrDefault);

        if (HasChildByMenuId(menuId))
        {
            throw new BusinessException("存在子菜单,不允许删除");
        }

        if (CheckMenuExistRole(singleOrDefault))
        {
            throw new BusinessException("菜单已分配,不允许删除");
        }

        _dbContext.Menus.Remove(singleOrDefault);
        _dbContext.SaveChanges();
    }


    public int ChangeSortMenu(int menuId, int sort)
    {
        var firstOrDefault = _dbContext.Menus.FirstOrDefault(x => x.Id == menuId);
        firstOrDefault = CheckMenuNullable(firstOrDefault);

        firstOrDefault.OrderNum = sort;
        _dbContext.Update(firstOrDefault);
        return _dbContext.SaveChanges();
    }


    /// <summary>
    /// 查询用户的菜单权限
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    /// <exception cref="BusinessException"></exception>
    public List<MenuResponse> SelectMenuTreeByUserId(int userId)
    {
        var firstOrDefault = _dbContext.Users.Include(x => x.Roles).FirstOrDefault(x => x.Id == userId);
        if (firstOrDefault == null)
        {
            throw new BusinessException("未找到此用户");
        }

        if (!firstOrDefault.Roles.Any() && !firstOrDefault.IsAdmin())
        {
            throw new BusinessException("此用户未设置权限信息");
        }

        if (firstOrDefault.IsAdmin())
        {
            var meniListAdmin = _dbContext.Roles.Include(x => x.Menus).Select(x => x.Menus).ToList();
            //转换成树形
            return TranslationTree(meniListAdmin.Adapt<List<MenuResponse>>());
        }

        var roleId = firstOrDefault.Roles.Select(x => x.Id);
        var rolesMenu = _dbContext.Roles.Include(x => x.Menus).Where(x => roleId.Contains(x.Id));
        var meniList = rolesMenu.Select(x => x.Menus).ToList();
        //转换成树形
        return TranslationTree(meniList.Adapt<List<MenuResponse>>());
    }

    /// <summary>
    /// 把列表转换成树
    /// </summary>
    /// <param name="menuResponses"></param>
    /// <returns></returns>
    private static List<MenuResponse> TranslationTree(List<MenuResponse> menuResponses)
    {
        var responses = new List<MenuResponse>();
        //map
        var data = menuResponses.ToDictionary(x => x.Id, x => x);

        foreach (var menuResponse in menuResponses)
        {
            if (menuResponse.ParentId == 0)
            {
                responses.Add(data[menuResponse.Id]);
            }
            else
            {
                var response = data[menuResponse.ParentId];
                response.Children.Adapt(response);
            }
        }

        return responses;
    }

    /// <summary>
    /// 检查是否有子菜单
    /// </summary>
    /// <param name="menuId"></param>
    /// <returns></returns>
    private bool HasChildByMenuId(int menuId)
    {
        return _dbContext.Menus.Any(x => x.ParentId == menuId);
    }

    /// <summary>
    /// 检查是否有角色关联
    /// </summary>
    /// <param name="menus"></param>
    /// <returns></returns>
    private static bool CheckMenuExistRole(Menu menus)
    {
        return menus.Roles.Any();
    }

    private bool CheckMenuNameUnique(int menuId, string menuName)
    {
        return _dbContext.Menus.Any(x => x.MenuName == menuName && x.Id != menuId);
    }

    private Menu CheckMenuNullable(Menu? menu)
    {
        if (menu == null)
        {
            throw new BusinessException("未找到此订单");
        }

        return menu;
    }
}