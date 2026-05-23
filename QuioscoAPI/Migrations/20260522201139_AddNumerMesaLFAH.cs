using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuioscoAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddNumerMesaLFAH : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "numer_mesa",
                table: "pedidos",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "numer_mesa",
                table: "pedidos");
        }
    }
}
