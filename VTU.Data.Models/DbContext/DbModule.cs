using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace VTU.Data.Models;

public interface DbModule
{
    void OnModelCreating(ModelBuilder modelBuilder);
}

public static class DbModuleServiceCollectionExtensions
{
    public static void AddDbModule<T>(this IServiceCollection services)
        where T : class, DbModule
    {
        services.AddSingleton<DbModule, T>();
    }
}