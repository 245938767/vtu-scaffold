using Microsoft.AspNetCore.Mvc;
using VTU.BaseApi.Controller;
using VTU.Data.Models.Users;
using VTU.Infrastructure.Models;
using VTU.Models.Request.Users;
using VTU.Service.Users;

namespace VTU.WebApi.Controllers;

[ApiController]
[Route("[controller]/v1")]
public class UserController : BaseController
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("userInfo")]
    // [PermissionFilter("user:list")]
    public JsonObject<PagedInfo<User>> getUserinfo([FromBody] UserQueryRequest userQueryRequest)
    {
        return _userService.getUserList(userQueryRequest);
    }
}