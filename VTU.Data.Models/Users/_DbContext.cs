using Microsoft.EntityFrameworkCore;
using VTU.Data.Models.Roles;
using VTU.Data.Models.Users;
using VTU.Infrastructure.Enums;

namespace VTU.Data.Models;

partial class EntityDbContext
{
    public DbSet<User> Users { get; set; }
}

public class UserDbModule : DbModule
{
    public void OnModelCreating(ModelBuilder modelBuilder)
    {
        var userModel = modelBuilder.Entity<User>();
        userModel.HasMany<Role>(e => e.Roles)
            .WithMany(e => e.Users)
            .UsingEntity(x =>
                x.ToTable("UserRoleTable").HasData(
                    new Dictionary<string, object>() { { "RolesId", 1 }, { "UsersId", 1 } }
                )
            );

        userModel.HasData(InitUser());
    }

    private static User InitUser()
    {
        var user = new User
        {
            Id = 1,
            CreateDateTime = default,
            UpdateDateTime = default,
            UserName = "admin",
            NickName = "admin",
            Email = "dw@xmail.com",
            Phonenumber = "12345678909",
            Gender = "1",
            Status = ValidStatus.UnValid,
            DelFlag = ValidStatus.UnValid,
            LoginIP = null,
            LoginDate = null,
            Roles = null,
        };
        user.setPassword("admin123");
        return user;
    }
}