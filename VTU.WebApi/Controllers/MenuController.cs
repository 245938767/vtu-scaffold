using Microsoft.AspNetCore.Mvc;
using VTU.BaseApi.Controller;
using VTU.Infrastructure.Models;
using VTU.Models.Request.Menus;
using VTU.Models.Response.Menus;
using VTU.Service.Filters;

namespace VTU.WebApi.Controllers;

[Route("[controller]/v1")]
[Verify]
public class MenuController : BaseController
{
    /// <summary>
    /// 获取菜单列表
    /// </summary>
    /// <returns></returns>
    [HttpGet("list")]
    public JsonObject<List<MenuResponse>> TreeMenuList()
    {
        var menuResponses = new List<MenuResponse>();
        return menuResponses;
    }

    /// <summary>
    /// 根据菜单编号获取详细信息
    /// </summary>
    /// <param name="menuId"></param>
    /// <returns></returns>
    [HttpGet("{menuId}")]
    public JsonObject<string> GetMenuInfo(int menuId = 0)
    {
        return "";
    }

    /// <summary>
    /// 根据菜单编号获取菜单列表，菜单管理首次进入
    /// </summary>
    /// <param name="menuId"></param>
    /// <returns></returns>
    [HttpGet("list/{menuId}")]
    public JsonObject<string> GetMenuList(int menuId = 0)
    {
        return "";
    }

    /// <summary>
    /// 获取角色菜单信息
    /// 加载对应角色菜单列表树
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>        
    [HttpGet("roleMenuTreeselect/{roleId}")]
    public JsonObject<string> RoleMenuTreeselect(int roleId)
    {
        return "";
    }

    /// <summary>
    /// 修改菜单
    /// </summary>
    /// <param name="menuDto"></param>
    /// <returns></returns>
    [HttpPost("edit")]
    public JsonObject<string> MenuEdit()
    {
        return "";
    }

    /// <summary>
    /// 添加菜单
    /// </summary>
    /// <param name="createMenuRequest"></param>
    /// <returns></returns>
    [HttpPut("add")]
    public JsonObject<string> MenuAdd([FromBody] CreateMenuRequest createMenuRequest)
    {
        return "";
    }

    /// <summary>
    /// 菜单删除
    /// </summary>
    /// <param name="menuId"></param>
    /// <returns></returns>
    [HttpDelete("{menuId}")]
    public JsonObject<string> Remove(int menuId = 0)
    {
        return "";
    }

    /// <summary>
    /// 保存排序
    /// </summary>
    /// <param name="id"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    [HttpGet("ChangeSort")]
    public JsonObject<string> ChangeSort(int id = 0, int value = 0)
    {
        return "";
    }
}