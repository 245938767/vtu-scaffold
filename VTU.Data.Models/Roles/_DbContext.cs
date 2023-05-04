using Microsoft.EntityFrameworkCore;
using VTU.Data.Models.Menus;
using VTU.Data.Models.Roles;
using VTU.Infrastructure.Constant;
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
        roleModel.HasData(InitRole());
    }

    private static Role InitRole()
    {
        return
            new()
            {
                Id = 1,
                CreateDateTime = DateTime.Now,
                UpdateDateTime = DateTime.Now,
                RoleName = "管理员",
                RoleKey = GlobalConstant.AdminRole,
                RoleSort = 0,
                Status = ValidStatus.Valid,
                DelFlag = ValidStatus.UnValid,
                DataScope = "1",
                MenuCheckStrictly = false,
                DeptCheckStrictly = false,
            };
    }
}