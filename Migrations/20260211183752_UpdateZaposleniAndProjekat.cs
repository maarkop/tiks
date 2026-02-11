using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZaposleniAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateZaposleniAndProjekat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Naziv",
                table: "Projekti");

            migrationBuilder.AddColumn<string>(
                name: "BrojTelefona",
                table: "Zaposleni",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Zaposleni",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BrojTelefona",
                table: "Zaposleni");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Zaposleni");

            migrationBuilder.AddColumn<string>(
                name: "Naziv",
                table: "Projekti",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
