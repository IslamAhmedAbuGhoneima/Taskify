using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CleanArchitecture.Infrastructure.Migrations.SqlServer
{
    /// <inheritdoc />
    public partial class RenamePhotoURLColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "533d3e10-7114-4609-8e43-05f9e0625321");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f6ca2c1b-0b63-4912-aab9-70cd4beeae0f");

            migrationBuilder.RenameColumn(
                name: "PhtotURL",
                table: "AspNetUsers",
                newName: "PhotoURL");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "05ffb40a-db02-4d8a-ad5a-7953a51a1cec", null, "admin", "ADMIN" },
                    { "154ecb56-87e1-426e-a13d-a4c2ce1d0d5c", null, "member", "MEMBER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "05ffb40a-db02-4d8a-ad5a-7953a51a1cec");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "154ecb56-87e1-426e-a13d-a4c2ce1d0d5c");

            migrationBuilder.RenameColumn(
                name: "PhotoURL",
                table: "AspNetUsers",
                newName: "PhtotURL");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "533d3e10-7114-4609-8e43-05f9e0625321", null, "admin", "ADMIN" },
                    { "f6ca2c1b-0b63-4912-aab9-70cd4beeae0f", null, "member", "MEMBER" }
                });
        }
    }
}
