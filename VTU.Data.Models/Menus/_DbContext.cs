using Microsoft.EntityFrameworkCore;
using VTU.Data.Models.Menus;
using VTU.Infrastructure.Enums;

namespace VTU.Data.Models;

partial class EntityDbContext
{
    public DbSet<Menu> Menus { set; get; }
}

public class MenuDbModule : DbModule
{
    public void OnModelCreating(ModelBuilder modelBuilder)
    {
        var menuModel = modelBuilder.Entity<Menu>().HasData(InitRole());
    }

    private static Menu InitRole()
    {
        return
            new()
            {
                Id = 1,
                CreateDateTime = DateTime.Now,
                UpdateDateTime = DateTime.Now,
                MenuName = "系统设置",
                ParentId = 0,
                OrderNum = 0,
                Path = "/system",
                Component = "",
                IsCache = ValidStatus.UnValid,
                IsFrame = ValidStatus.UnValid,
                MenuType = "M",
                Visible = ValidStatus.Valid,
                Status = ValidStatus.Valid,
                Perms = "",
                Icon = null,
                MenuNameKey = "system",
                SubNum = 0,
            };
    }
}