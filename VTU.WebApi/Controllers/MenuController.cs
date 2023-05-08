using Microsoft.AspNetCore.Mvc;
using VTU.BaseApi.Controller;
using VTU.Infrastructure.Models;
using VTU.Models.Request.Menus;
using VTU.Models.Response.Menus;
using VTU.Service.Filters;
using VTU.Service.Menus;

namespace VTU.WebApi.Controllers;

[Route("[controller]/v1")]
[Verify]
public class MenuController : BaseController
{
    private readonly IMenuService _menuService;

    public MenuController(IMenuService menuService)
    {
        _menuService = menuService;
    }

    /// <summary>
    /// 获取菜单列表
    /// </summary>
    /// <returns></returns>
    [HttpGet("list")]
    public JsonObject<List<MenuResponse>> TreeMenuList()
    {
        return _menuService.SelectTreeMenuList();
    }

    /// <summary>
    /// 根据菜单编号获取详细信息
    /// </summary>
    /// <param name="menuId"></param>
    /// <returns></returns>
    [HttpGet("{menuId}")]
    public JsonObject<MenuResponse> GetMenuInfo(int menuId = 0)
    {
        return _menuService.GetMenuByMenuId(menuId);
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
    /// 修改菜单
    /// </summary>
    /// <param name="editMenuRequest"></param>
    /// <returns></returns>
    [HttpPost("edit")]
    public JsonObject<string> MenuEdit([FromBody] EditMenuRequest editMenuRequest)
    {
        _menuService.EditMenu(editMenuRequest);
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
        _menuService.AddMenu(createMenuRequest);
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
        _menuService.DeleteMenuById(menuId);
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
        _menuService.ChangeSortMenu(id, value);
        return "";
    }
}