using Microsoft.AspNetCore.Mvc;
using VTU.BaseApi.Controller;
using VTU.Infrastructure.Models;
using VTU.Models.Request.Roles;
using VTU.Models.Response.Roles;
using VTU.Service.Filters;
using VTU.Service.Users;

namespace VTU.WebApi.Controllers;

[Route("[controller]/v1")]
[Verify]
public class RoleController : BaseController
{
    private readonly IRoleService _roleService;

    [HttpGet("/list")]
    [PermissionFilter("role:list")]
    public JsonObject<PagedInfo<RoleResponse>> List([FromQuery] RoleQueryRequest roleQueryRequest)
    {
        return _roleService.SelectRoleList(roleQueryRequest);
    }

    [HttpGet("/info/{roleId:long}")]
    public JsonObject<RoleResponse> GetInfo(long roleId = 0)
    {
        return _roleService.SelectRoleById(roleId);
    }

    [HttpPost("/create")]
    [PermissionFilter("role:create")]
    public JsonObject<string> CreateRole([FromBody] CreateRoleRequest createRoleRequest)
    {
        return _roleService.InsertRole(createRoleRequest).ToString();
    }
}