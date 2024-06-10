using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SqlServerMigrations.Migrations
{
    public partial class UserData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "114fcebd-a934-479c-aa42-48d5d84e0670", "ff5a822b-505d-4a45-8790-906dd5d363d7", "NewsManager", "NEWSMANAGER" },
                    { "32098694-1c75-46d3-87a8-da162dab0335", "48f2fe6c-f12c-4788-ab94-d016b5b310dd", "SuperAdministrator", "SUPERADMINISTRATOR" },
                    { "4e9c0350-32c1-464f-b41a-2d0b595a0a8c", "e28a5cc4-fcdf-4836-9b63-dae26ad56597", "CommentManager", "COMMENTMANAGER" },
                    { "874a001d-3ef4-49af-a579-844c9a1034b4", "2856cf5b-9af9-40ba-bcd3-013e810e92c7", "Administrator", "ADMINISTRATOR" },
                    { "c91f0ea1-9279-46fb-bb9e-f70b0879a0c7", "1bc8e192-3565-4813-aa17-9d66657bff54", "ProductManager", "PRODUCTMANAGER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "BirthDate", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "87f7d358-de81-415b-a498-b08e0cf90636", 0, new DateTime(1997, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "e612a8c8-7555-47b5-be79-59a4665c0559", "admin@badwolf.tech", true, "Admin", "Admin", false, null, "ADMIN@BADWOLF.TECH", "Admin", "AQAAAAEAACcQAAAAEAXUc1eWMEZ7n2TsbY90ntrTdMSUyhHXMuBRMiNLbAZIV6BQStACnB8c6mMGFoVUhg==", null, false, "542e803a-4ba7-4d5e-a5cc-f0443dd02368", false, "Admin" });

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "Id", "CodePage", "Contents", "Created", "Discriminator", "ImageName", "IsDelete", "IsView", "Text", "Title" },
                values: new object[,]
                {
                    { new Guid("7bda8276-759d-4bbb-b795-104b2d2c5235"), "Privacy", "[]", new DateTime(2024, 5, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Post", null, false, true, "Информация заполняется администратором.", "Конфиденциальность" },
                    { new Guid("88494cd9-a407-4ce6-aff4-8e5e25a1df5a"), "Cookies", "[]", new DateTime(2024, 5, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Post", null, false, true, "Информация заполняется администратором.", "Cookies" },
                    { new Guid("b43d176a-6cda-46ac-aaa4-7b8720d0393d"), "Index", "[]", new DateTime(2024, 5, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Post", null, false, true, "Информация заполняется администратором.", "Главная" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "32098694-1c75-46d3-87a8-da162dab0335", "87f7d358-de81-415b-a498-b08e0cf90636" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "874a001d-3ef4-49af-a579-844c9a1034b4", "87f7d358-de81-415b-a498-b08e0cf90636" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "114fcebd-a934-479c-aa42-48d5d84e0670");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4e9c0350-32c1-464f-b41a-2d0b595a0a8c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c91f0ea1-9279-46fb-bb9e-f70b0879a0c7");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "32098694-1c75-46d3-87a8-da162dab0335", "87f7d358-de81-415b-a498-b08e0cf90636" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "874a001d-3ef4-49af-a579-844c9a1034b4", "87f7d358-de81-415b-a498-b08e0cf90636" });

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("7bda8276-759d-4bbb-b795-104b2d2c5235"));

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("88494cd9-a407-4ce6-aff4-8e5e25a1df5a"));

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("b43d176a-6cda-46ac-aaa4-7b8720d0393d"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "32098694-1c75-46d3-87a8-da162dab0335");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "874a001d-3ef4-49af-a579-844c9a1034b4");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "87f7d358-de81-415b-a498-b08e0cf90636");
        }
    }
}
