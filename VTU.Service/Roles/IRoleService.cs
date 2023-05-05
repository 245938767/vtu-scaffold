using VTU.Data.Models.Roles;
using VTU.Infrastructure.Models;
using VTU.Models.Request.Roles;
using VTU.Models.Response.Roles;

namespace VTU.Service.Roles;

public interface IRoleService
{
    /// <summary>
    /// 根据条件分页查询角色数据
    /// </summary>
    /// <param name="roleQueryRequest"></param>
    /// <returns>角色数据集合信息</returns>
    public PagedInfo<RoleResponse> SelectRoleList(RoleQueryRequest roleQueryRequest);

    /// <summary>
    /// 查询所有角色
    /// </summary>
    /// <returns></returns>
    public List<RoleResponse> SelectRoleAll();


    /// <summary>
    /// 通过角色ID查询角色
    /// </summary>
    /// <param name="roleId">角色ID</param>
    /// <returns>角色对象信息</returns>
    public RoleResponse SelectRoleById(long roleId);

    /// <summary>
    /// 批量删除角色信息
    /// </summary>
    /// <param name="roleIds">需要删除的角色ID</param>
    /// <returns></returns>
    public int DeleteRoleByRoleId(long[] roleIds);

    /// <summary>
    /// 更改角色权限状态
    /// </summary>
    /// <param name="roleDto"></param>
    /// <returns></returns>
    public int UpdateRoleStatus(Role roleDto);


    /// <summary>
    /// 校验角色是否允许操作
    /// </summary>
    /// <param name="role"></param>
    public void CheckRoleAllowed(Role role);

    /// <summary>
    /// 新增保存角色信息
    /// </summary>
    /// <param name="createRoleRequest"></param>
    /// <returns></returns>
    public long InsertRole(CreateRoleRequest createRoleRequest);


    #region Service

    /// <summary>
    /// 获取角色菜单id集合
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    public List<long> SelectUserRoleMenus(long roleId);

    List<long> SelectRoleMenuByRoleIds(long[] roleIds);


    /// <summary>
    /// 获取用户权限集合
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public List<long> SelectUserRoles(long userId);

    /// <summary>
    /// 获取用户权限字符串集合
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public List<string> SelectUserRoleKeys(long userId);

    public List<string> SelectUserRoleNames(long userId);

    /// <summary>
    /// 修改保存角色信息
    /// </summary>
    /// <param name="editRoleRequest">角色信息</param>
    /// <returns></returns>
    public int UpdateRole(EditRoleRequest editRoleRequest);

    #endregion
}