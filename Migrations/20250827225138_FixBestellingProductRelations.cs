using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NET_Advanced.Migrations
{
    /// <inheritdoc />
    public partial class FixBestellingProductRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BestellingProducten_Bestellingen_BestellingModelId",
                table: "BestellingProducten");

            migrationBuilder.DropIndex(
                name: "IX_BestellingProducten_BestellingModelId",
                table: "BestellingProducten");

            migrationBuilder.DropColumn(
                name: "BestellingModelId",
                table: "BestellingProducten");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BestellingModelId",
                table: "BestellingProducten",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BestellingProducten_BestellingModelId",
                table: "BestellingProducten",
                column: "BestellingModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_BestellingProducten_Bestellingen_BestellingModelId",
                table: "BestellingProducten",
                column: "BestellingModelId",
                principalTable: "Bestellingen",
                principalColumn: "Id");
        }
    }
}
