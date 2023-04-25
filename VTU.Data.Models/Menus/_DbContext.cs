using Microsoft.EntityFrameworkCore;
using VTU.Data.Models.Menus;

namespace VTU.Data.Models;

partial class EntityDbContext
{
    public DbSet<Menu> Menus { set; get; }
}

public class MenuDbModule : DbModule
{
    public void OnModelCreating(ModelBuilder modelBuilder)
    {
        var menuModel = modelBuilder.Entity<Menu>();
    }
}