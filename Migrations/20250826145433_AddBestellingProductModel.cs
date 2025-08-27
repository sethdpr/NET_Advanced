using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NET_Advanced.Migrations
{
    /// <inheritdoc />
    public partial class AddBestellingProductModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DatumBetaald",
                table: "Bestellingen",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DatumGemaakt",
                table: "Bestellingen",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "GemaaktDoor",
                table: "Bestellingen",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "BestellingProducten",
                columns: table => new
                {
                    BestellingId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProductId = table.Column<int>(type: "INTEGER", nullable: false),
                    Aantal = table.Column<int>(type: "INTEGER", nullable: false),
                    Prijs = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BestellingModelId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BestellingProducten", x => new { x.BestellingId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_BestellingProducten_Bestellingen_BestellingId",
                        column: x => x.BestellingId,
                        principalTable: "Bestellingen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BestellingProducten_Bestellingen_BestellingModelId",
                        column: x => x.BestellingModelId,
                        principalTable: "Bestellingen",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BestellingProducten_Producten_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Producten",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BestellingProducten_BestellingModelId",
                table: "BestellingProducten",
                column: "BestellingModelId");

            migrationBuilder.CreateIndex(
                name: "IX_BestellingProducten_ProductId",
                table: "BestellingProducten",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BestellingProducten");

            migrationBuilder.DropColumn(
                name: "DatumBetaald",
                table: "Bestellingen");

            migrationBuilder.DropColumn(
                name: "DatumGemaakt",
                table: "Bestellingen");

            migrationBuilder.DropColumn(
                name: "GemaaktDoor",
                table: "Bestellingen");
        }
    }
}
