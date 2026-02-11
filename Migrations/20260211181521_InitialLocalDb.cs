using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ZaposleniAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialLocalDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Plate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ZaposleniJmbg = table.Column<string>(type: "text", nullable: false),
                    Iznos = table.Column<decimal>(type: "numeric", nullable: false),
                    DatumIsplate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Zaposleni",
                columns: table => new
                {
                    Jmbg = table.Column<string>(type: "text", nullable: false),
                    Ime = table.Column<string>(type: "text", nullable: false),
                    Prezime = table.Column<string>(type: "text", nullable: false),
                    DatumRodjenja = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BrojTelefona = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zaposleni", x => x.Jmbg);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Plate");

            migrationBuilder.DropTable(
                name: "Zaposleni");
        }
    }
}
