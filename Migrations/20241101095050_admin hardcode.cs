using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcommercePetsFoodBackend.Migrations
{
    /// <inheritdoc />
    public partial class adminhardcode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "Email", "IsBlocked", "Name", "Password", "Phone", "Role" },
                values: new object[] { 1, "admin@example.com", false, "Admin User", "$2a$11$nZotRaLko3rwi3mr6LKRVOMecSh4G9WFbei3AVft8SVtnDyTVFDla", 0L, "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
