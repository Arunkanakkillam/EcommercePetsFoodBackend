using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcommercePetsFoodBackend.Migrations
{
    /// <inheritdoc />
    public partial class adminhardcoded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$WQFadKt0BxZuRQNLN.3HeeT509zJv7sdhIumUMWC3CS7WjYRtq2FC");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$nZotRaLko3rwi3mr6LKRVOMecSh4G9WFbei3AVft8SVtnDyTVFDla");
        }
    }
}
