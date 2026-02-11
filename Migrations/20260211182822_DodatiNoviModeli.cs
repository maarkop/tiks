using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ZaposleniAPI.Migrations
{
    /// <inheritdoc />
    public partial class DodatiNoviModeli : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BrojTelefona",
                table: "Zaposleni");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Zaposleni");

            migrationBuilder.CreateTable(
                name: "Pozicije",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ZaposleniJmbg = table.Column<string>(type: "text", nullable: false),
                    NazivPozicije = table.Column<string>(type: "text", nullable: false),
                    DatumOd = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DatumDo = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pozicije", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Projekti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MenadzerJmbg = table.Column<string>(type: "text", nullable: false),
                    DatumOd = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DatumDo = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Naziv = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projekti", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RadioNa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProjekatId = table.Column<int>(type: "integer", nullable: false),
                    PozicijaId = table.Column<int>(type: "integer", nullable: false),
                    Opis = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RadioNa", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pozicije");

            migrationBuilder.DropTable(
                name: "Projekti");

            migrationBuilder.DropTable(
                name: "RadioNa");

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
    }
}
