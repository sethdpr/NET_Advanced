using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NET_Advanced.Migrations
{
    /// <inheritdoc />
    public partial class AddGemaaktDoorAndAangemaaktOp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DatumBetaald",
                table: "Bestellingen");

            migrationBuilder.RenameColumn(
                name: "DatumGemaakt",
                table: "Bestellingen",
                newName: "AangemaaktOp");

            migrationBuilder.AlterColumn<string>(
                name: "GemaaktDoor",
                table: "Bestellingen",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AangemaaktOp",
                table: "Bestellingen",
                newName: "DatumGemaakt");

            migrationBuilder.AlterColumn<string>(
                name: "GemaaktDoor",
                table: "Bestellingen",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DatumBetaald",
                table: "Bestellingen",
                type: "TEXT",
                nullable: true);
        }
    }
}
