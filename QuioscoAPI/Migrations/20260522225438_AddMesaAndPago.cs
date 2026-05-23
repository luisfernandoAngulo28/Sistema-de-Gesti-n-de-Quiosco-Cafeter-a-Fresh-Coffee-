using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace QuioscoAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddMesaAndPago : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "numer_mesa",
                table: "pedidos",
                newName: "mesa_id");

            migrationBuilder.CreateTable(
                name: "mesas",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    numero = table.Column<int>(type: "integer", nullable: false),
                    capacidad = table.Column<int>(type: "integer", nullable: false),
                    disponible = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mesas", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "pagos",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    pedido_id = table.Column<int>(type: "integer", nullable: false),
                    metodo = table.Column<string>(type: "text", nullable: false),
                    monto = table.Column<double>(type: "double precision", nullable: false),
                    fecha_pago = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pagos", x => x.id);
                    table.ForeignKey(
                        name: "FK_pagos_pedidos_pedido_id",
                        column: x => x.pedido_id,
                        principalTable: "pedidos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_pedidos_mesa_id",
                table: "pedidos",
                column: "mesa_id");

            migrationBuilder.CreateIndex(
                name: "IX_mesas_numero",
                table: "mesas",
                column: "numero",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_pagos_pedido_id",
                table: "pagos",
                column: "pedido_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_pedidos_mesas_mesa_id",
                table: "pedidos",
                column: "mesa_id",
                principalTable: "mesas",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_pedidos_mesas_mesa_id",
                table: "pedidos");

            migrationBuilder.DropTable(
                name: "mesas");

            migrationBuilder.DropTable(
                name: "pagos");

            migrationBuilder.DropIndex(
                name: "IX_pedidos_mesa_id",
                table: "pedidos");

            migrationBuilder.RenameColumn(
                name: "mesa_id",
                table: "pedidos",
                newName: "numer_mesa");
        }
    }
}
