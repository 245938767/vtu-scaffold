using VTU.Infrastructure.Attribute;

namespace VTU.Service.Menus;

[Service(ServiceType = typeof(IMenuService), ServiceLifetime = LifeTime.Transient)]
public class MenuServiceImpl : IMenuService
{
}