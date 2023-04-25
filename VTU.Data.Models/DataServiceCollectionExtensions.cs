using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VTU.Data.Models.Roles;
using VTU.Infrastructure;

namespace VTU.Data.Models;

public static class DataServiceCollectionExtensions
{
    /**
         * Other SQL install 
         * https://learn.microsoft.com/zh-cn/ef/core/providers/?tabs=dotnet-core-cli
         */
    public static DbContextOptionsBuilder GetSql(DbContextOptionsBuilder dbContextOptionsBuilder)
    {
        var connectionStrings = AppSettingInstants.GetAppSettings().ConnectionStrings;
        switch (connectionStrings.SQLType)
        {
            case 0:
                dbContextOptionsBuilder.UseMySql(connectionStrings.MySQL,
                    new MySqlServerVersion(new Version(8, 0, 31)));
                break;
            case 1:
                dbContextOptionsBuilder.UseSqlite(connectionStrings.SQLite);
                break;
            case 2:
                dbContextOptionsBuilder.UseSqlServer(connectionStrings.SQLServer);
                break;
        }

        return dbContextOptionsBuilder;
    }

    /**
     * 添加对应的Module对象
     */
    public static void AddDbModules(this IServiceCollection services)
    {
        services.AddDbModule<UserDbModule>();
        services.AddDbModule<RoleDbModule>();
        services.AddDbModule<MenuDbModule>();
    }
}