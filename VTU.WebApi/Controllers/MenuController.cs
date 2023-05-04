using Microsoft.AspNetCore.Mvc;
using VTU.BaseApi.Controller;
using VTU.Service.Filters;

namespace VTU.WebApi.Controllers;

[Route("[controller]/v1")]
[Verify]
public class MenuController : BaseController
{
}