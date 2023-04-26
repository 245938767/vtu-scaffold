using Microsoft.AspNetCore.Mvc;
using VTU.BaseApi.Controller;
using VTU.Data.Models;
using VTU.Data.Models.Users;
using VTU.Infrastructure.Exceptions;
using VTU.Infrastructure.Models;

namespace VTU.WebApi.Controllers;

[ApiController]
[Route("[controller]/v1")]
public class UserController : BaseController
{
    private readonly EntityDbContext _dbContext;

    public UserController(EntityDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("userInfo")]
    // [PermissionFilter("user:list")]
    public JsonObject<ICollection<User>> getUserinfo(int page = 1, int pageSize = 20)
    {
        var queryable = _dbContext.Users.AsQueryable().OrderByDescending(x => x.Id).Skip((page - 1) * pageSize)
            .Take(pageSize).ToList();
        return queryable;
    }
}