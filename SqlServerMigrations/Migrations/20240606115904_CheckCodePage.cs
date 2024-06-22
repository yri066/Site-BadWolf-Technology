using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SqlServerMigrations.Migrations
{
    public partial class CheckCodePage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR ALTER VIEW View_PostCodePages AS
                SELECT Id, CodePage
                FROM Posts
            ");

            migrationBuilder.DropTable(
                name: "Post");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "114fcebd-a934-479c-aa42-48d5d84e0670",
                column: "ConcurrencyStamp",
                value: "aa5cfcc8-ed88-461a-b6fc-38bcb9d436ba");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "32098694-1c75-46d3-87a8-da162dab0335",
                column: "ConcurrencyStamp",
                value: "84f80948-1f5d-4d78-9f91-75c63d062933");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4e9c0350-32c1-464f-b41a-2d0b595a0a8c",
                column: "ConcurrencyStamp",
                value: "513babcf-04e7-4a2a-b639-2a9f7e97a9f6");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "874a001d-3ef4-49af-a579-844c9a1034b4",
                column: "ConcurrencyStamp",
                value: "d91aca47-9eec-4834-822d-9b0b2f495431");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c91f0ea1-9279-46fb-bb9e-f70b0879a0c7",
                column: "ConcurrencyStamp",
                value: "e75f9d09-da8d-4063-aa2f-dd377fd6ff19");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "87f7d358-de81-415b-a498-b08e0cf90636",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "30a68933-2a12-4d35-9add-0e839b3f123d", "AQAAAAEAACcQAAAAELvkdUB6U1cUB0BGWOlH7ymEh7kxe1XceCcZRYLIsS2iP0hPSGhRTYDsMxN7nY9FZA==", "c04cad1b-b118-44eb-b63e-40eab3ff8bb3" });

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("7bda8276-759d-4bbb-b795-104b2d2c5235"),
                column: "Contents",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("88494cd9-a407-4ce6-aff4-8e5e25a1df5a"),
                column: "Contents",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("b43d176a-6cda-46ac-aaa4-7b8720d0393d"),
                column: "Contents",
                value: "[]");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                drop view View_PostCodePages;
            ");
            migrationBuilder.CreateTable(
                name: "Post",
                columns: table => new
                {
                    CodePage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "114fcebd-a934-479c-aa42-48d5d84e0670",
                column: "ConcurrencyStamp",
                value: "075e6439-3fbe-4306-a60a-ee366bead180");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "32098694-1c75-46d3-87a8-da162dab0335",
                column: "ConcurrencyStamp",
                value: "2b7441fd-c13a-442d-83c8-fb5705bc59f8");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4e9c0350-32c1-464f-b41a-2d0b595a0a8c",
                column: "ConcurrencyStamp",
                value: "e3990cab-6aa9-4a02-99a1-4d3013102dfa");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "874a001d-3ef4-49af-a579-844c9a1034b4",
                column: "ConcurrencyStamp",
                value: "49ef2a27-c427-430b-a9f3-e62ff68a6fec");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c91f0ea1-9279-46fb-bb9e-f70b0879a0c7",
                column: "ConcurrencyStamp",
                value: "93aa62f5-1e01-49b1-8bad-692312c2d765");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "87f7d358-de81-415b-a498-b08e0cf90636",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9b98702b-6051-467b-9465-80a3241c2ae6", "AQAAAAEAACcQAAAAEF49OC7RFp4c+AwIYnPHEdKQzt1m9WyiGDTWh+X604azXdxDQbsrxi+otFvBjFWmYQ==", "55747b94-ce03-4f5e-8608-0ffc6d519fdd" });
        }
    }
}
