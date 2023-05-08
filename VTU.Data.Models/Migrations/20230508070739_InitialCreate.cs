using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VTU.Data.Models.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "t_menu",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "ID")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MenuName = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "菜单名称"),
                    ParentId = table.Column<int>(type: "int", nullable: true, comment: "父Id"),
                    OrderNum = table.Column<int>(type: "int", nullable: false, comment: "显示顺序"),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "路由地址"),
                    Component = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "组件路径"),
                    IsCache = table.Column<int>(type: "int", nullable: false, comment: "是否缓存(0=禁用,1=启用)"),
                    IsFrame = table.Column<int>(type: "int", nullable: false, comment: "是否外链(0=禁用,1=启用)"),
                    MenuType = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "类型（M目录 C菜单 F按钮 L链接）"),
                    Visible = table.Column<int>(type: "int", nullable: false, comment: "显示状态(0=禁用,1=启用)"),
                    Status = table.Column<int>(type: "int", nullable: false, comment: "菜单状态(0=禁用,1=启用)"),
                    Perms = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "权限字符串"),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "菜单图标"),
                    MenuNameKey = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "菜单名key"),
                    SubNum = table.Column<int>(type: "int", nullable: false),
                    MenuId = table.Column<int>(type: "int", nullable: true),
                    CreateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "创建时间"),
                    UpdateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "更新时间时间"),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false, comment: "乐观锁")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_menu", x => x.Id);
                    table.ForeignKey(
                        name: "FK_t_menu_t_menu_MenuId",
                        column: x => x.MenuId,
                        principalTable: "t_menu",
                        principalColumn: "Id");
                },
                comment: "菜单表");

            migrationBuilder.CreateTable(
                name: "t_role",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "ID")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "角色名称"),
                    RoleKey = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "角色权限"),
                    RoleSort = table.Column<int>(type: "int", nullable: false, comment: "角色排序"),
                    Status = table.Column<int>(type: "int", nullable: false, comment: "角色状态(0=禁用,1=启用)"),
                    DelFlag = table.Column<int>(type: "int", nullable: false, comment: "删除标志(0=禁用,1=启用)"),
                    DataScope = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "数据范围（1：全部数据权限 2：自定数据权限 3：本部门数据权限 4：本部门及以下数据权限））"),
                    MenuCheckStrictly = table.Column<bool>(type: "bit", nullable: false, comment: "菜单树选择项是否关联显示"),
                    DeptCheckStrictly = table.Column<bool>(type: "bit", nullable: false, comment: "部门树选择项是否关联显示"),
                    CreateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "创建时间"),
                    UpdateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "更新时间时间"),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false, comment: "乐观锁")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_role", x => x.Id);
                },
                comment: "角色表");

            migrationBuilder.CreateTable(
                name: "t_user",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "ID")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NickName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Password"),
                    Phonenumber = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "手机号"),
                    Gender = table.Column<int>(type: "int", nullable: false, comment: "用户性别(0=男,1=女,2=未知)"),
                    Salt = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "密码加密的盐"),
                    Status = table.Column<int>(type: "int", nullable: false, comment: "帐号状态(0=禁用,1=启用)"),
                    DelFlag = table.Column<int>(type: "int", nullable: false, comment: "删除标志(0=禁用,1=启用)"),
                    LoginIP = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "最后登录IP"),
                    LoginDate = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "最后登录时间"),
                    CreateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "创建时间"),
                    UpdateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "更新时间时间"),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false, comment: "乐观锁")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_user", x => x.Id);
                },
                comment: "用户表");

            migrationBuilder.CreateTable(
                name: "t_RoleMenuTable",
                columns: table => new
                {
                    MenusId = table.Column<int>(type: "int", nullable: false),
                    RolesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_RoleMenuTable", x => new { x.MenusId, x.RolesId });
                    table.ForeignKey(
                        name: "FK_t_RoleMenuTable_t_menu_MenusId",
                        column: x => x.MenusId,
                        principalTable: "t_menu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_t_RoleMenuTable_t_role_RolesId",
                        column: x => x.RolesId,
                        principalTable: "t_role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "t_UserRoleTable",
                columns: table => new
                {
                    RolesId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_UserRoleTable", x => new { x.RolesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_t_UserRoleTable_t_role_RolesId",
                        column: x => x.RolesId,
                        principalTable: "t_role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_t_UserRoleTable_t_user_UsersId",
                        column: x => x.UsersId,
                        principalTable: "t_user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "t_menu",
                columns: new[] { "Id", "Component", "CreateDateTime", "Icon", "IsCache", "IsFrame", "MenuId", "MenuName", "MenuNameKey", "MenuType", "OrderNum", "ParentId", "Path", "Perms", "Status", "SubNum", "UpdateDateTime", "Visible" },
                values: new object[] { 1, "", new DateTime(2023, 5, 8, 15, 7, 39, 43, DateTimeKind.Local).AddTicks(1390), null, 0, 0, null, "系统设置", "system", "M", 0, 0, "/system", "", 1, 0, new DateTime(2023, 5, 8, 15, 7, 39, 43, DateTimeKind.Local).AddTicks(1390), 1 });

            migrationBuilder.InsertData(
                table: "t_role",
                columns: new[] { "Id", "CreateDateTime", "DataScope", "DelFlag", "DeptCheckStrictly", "MenuCheckStrictly", "RoleKey", "RoleName", "RoleSort", "Status", "UpdateDateTime" },
                values: new object[] { 1, new DateTime(2023, 5, 8, 15, 7, 39, 43, DateTimeKind.Local).AddTicks(1300), "1", 0, false, false, "admin", "管理员", 0, 1, new DateTime(2023, 5, 8, 15, 7, 39, 43, DateTimeKind.Local).AddTicks(1320) });

            migrationBuilder.InsertData(
                table: "t_user",
                columns: new[] { "Id", "CreateDateTime", "DelFlag", "Email", "Gender", "LoginDate", "LoginIP", "NickName", "Password", "Phonenumber", "Salt", "Status", "UpdateDateTime", "UserName" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "dw@xmail.com", 0, null, null, "admin", "FWiumVdXOqkWHzXCXwXGDqxBZPJ+32nJRjm665ZQA14=", "12345678909", "V4MNurlZRVEi2gBvhF3cXg==", 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin" });

            migrationBuilder.InsertData(
                table: "t_UserRoleTable",
                columns: new[] { "RolesId", "UsersId" },
                values: new object[] { 1, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_t_menu_MenuId",
                table: "t_menu",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_t_RoleMenuTable_RolesId",
                table: "t_RoleMenuTable",
                column: "RolesId");

            migrationBuilder.CreateIndex(
                name: "IX_t_UserRoleTable_UsersId",
                table: "t_UserRoleTable",
                column: "UsersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "t_RoleMenuTable");

            migrationBuilder.DropTable(
                name: "t_UserRoleTable");

            migrationBuilder.DropTable(
                name: "t_menu");

            migrationBuilder.DropTable(
                name: "t_role");

            migrationBuilder.DropTable(
                name: "t_user");
        }
    }
}
