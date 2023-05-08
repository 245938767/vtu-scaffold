using VTU.Models.Request.Menus;
using VTU.Models.Response.Menus;

namespace VTU.Service.Menus;

public interface IMenuService
{
    List<MenuResponse> SelectTreeMenuList();

    MenuResponse GetMenuByMenuId(int menuId);
    List<MenuResponse> GetMenusByMenuId(int menuId, int userId);
    int AddMenu(CreateMenuRequest menu);

    int EditMenu(EditMenuRequest menu);

    void DeleteMenuById(int menuId);


    int ChangeSortMenu(int menuId, int sort);

    public List<MenuResponse> SelectMenuTreeByUserId(int userId);
}