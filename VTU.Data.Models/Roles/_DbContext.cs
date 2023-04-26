using Microsoft.EntityFrameworkCore;
using VTU.Data.Models.Menus;
using VTU.Data.Models.Roles;
using VTU.Infrastructure.Enums;

namespace VTU.Data.Models;

partial class EntityDbContext
{
    public DbSet<Role> Roles { get; set; }
}

public class RoleDbModule : DbModule
{
    public void OnModelCreating(ModelBuilder modelBuilder)
    {
        var roleModel = modelBuilder.Entity<Role>();
        roleModel.HasMany<Menu>(e => e.Menus).WithMany(x => x.Roles)
            .UsingEntity("RoleMenuTable");
        roleModel.HasData(initRole());
    }

    private Role initRole()
    {
        return
            new()
            {
                Id = 1,
                CreateDateTime = DateTime.Now,
                UpdateDateTime = DateTime.Now,
                RoleName = "管理员",
                RoleKey = "*:*:*",
                RoleSort = 0,
                Status = ValidStatus.UnValid,
                DelFlag = ValidStatus.UnValid,
                DataScope = "1",
                MenuCheckStrictly = false,
                DeptCheckStrictly = false,
            };
    }
}