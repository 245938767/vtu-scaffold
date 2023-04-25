using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VTU.Data.Models;
using VTU.Infrastructure;

namespace VTU.Data.Migration;

public class DesignTimeDbContextFactory :
    IDesignTimeDbContextFactory<EntityDbContext>
{
    public EntityDbContext CreateDbContext(string[] args)
    {
        var envName = "Development";
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{envName}.json", true)
            .AddEnvironmentVariables()
            .Build();
        //初始化Setting
        var appSettingInstants = new AppSettingInstants(config);

        var options = DataServiceCollectionExtensions.GetSql(new DbContextOptionsBuilder<EntityDbContext>()).Options;
        var services = new ServiceCollection();
        services.AddDbModules();
        var sp = services.BuildServiceProvider();
        return ActivatorUtilities.CreateInstance<EntityDbContext>(sp, options);
    }
}