using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SqlServerMigrations.Migrations
{
    public partial class NewsSearchString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SearchString",
                table: "News",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "114fcebd-a934-479c-aa42-48d5d84e0670",
                column: "ConcurrencyStamp",
                value: "cab4ea0c-db49-400f-8571-5a4daea7a8c3");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "32098694-1c75-46d3-87a8-da162dab0335",
                column: "ConcurrencyStamp",
                value: "cd2656b2-78ed-4ee4-9e77-a935de2b1a2b");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4e9c0350-32c1-464f-b41a-2d0b595a0a8c",
                column: "ConcurrencyStamp",
                value: "83a78850-2042-4722-b12b-785b9e548216");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "874a001d-3ef4-49af-a579-844c9a1034b4",
                column: "ConcurrencyStamp",
                value: "b1f35085-4cfe-46a6-82bc-9d19ebeba4be");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c91f0ea1-9279-46fb-bb9e-f70b0879a0c7",
                column: "ConcurrencyStamp",
                value: "ac8df4f1-8c4c-4f7e-b29e-9835bef1bb61");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "87f7d358-de81-415b-a498-b08e0cf90636",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6428faa2-7646-4a94-8f88-20dfa09d2a50", "AQAAAAEAACcQAAAAEH5yWk5/c8YDmzZXZxZV5oS3K1mUUSIpHnElvjo6RbW1wABKjLDtIDoN2WW1tEB3pQ==", "b82a1215-02d2-42da-8b96-8df7b943bcee" });

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
            migrationBuilder.DropColumn(
                name: "SearchString",
                table: "News");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "114fcebd-a934-479c-aa42-48d5d84e0670",
                column: "ConcurrencyStamp",
                value: "4d0674fb-0630-4e8a-8322-4684cd70c1f3");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "32098694-1c75-46d3-87a8-da162dab0335",
                column: "ConcurrencyStamp",
                value: "49a60f82-ddb8-4455-8693-579d134dd960");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4e9c0350-32c1-464f-b41a-2d0b595a0a8c",
                column: "ConcurrencyStamp",
                value: "92459e34-f3f2-4ce4-accc-b202cefa68d0");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "874a001d-3ef4-49af-a579-844c9a1034b4",
                column: "ConcurrencyStamp",
                value: "e6741cf4-295c-4285-8290-c0b373e6ce21");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c91f0ea1-9279-46fb-bb9e-f70b0879a0c7",
                column: "ConcurrencyStamp",
                value: "3ab42317-9642-4ef3-a0a8-4a29b4d53905");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "87f7d358-de81-415b-a498-b08e0cf90636",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e7524319-1117-4b4f-b2e8-9d0032b56ef1", "AQAAAAEAACcQAAAAEIsTKtfi5dGcjOpHMlWqeoZTPvaTFj531yUPlvojD44lLOSQ1qG/p0vOz5Cq+GaqDQ==", "78a47df8-1052-405e-949f-e25eb04760e5" });
        }
    }
}
