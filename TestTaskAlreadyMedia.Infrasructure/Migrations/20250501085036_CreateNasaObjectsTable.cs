using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestTaskAlreadyMedia.Infrasructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateNasaObjectsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NasaObjects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    NasaId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    NameType = table.Column<string>(type: "text", nullable: false),
                    Recclass = table.Column<string>(type: "text", nullable: false),
                    Mass = table.Column<decimal>(type: "numeric", nullable: false),
                    Fall = table.Column<string>(type: "text", nullable: false),
                    Year = table.Column<short>(type: "smallint", nullable: false),
                    Reclat = table.Column<decimal>(type: "numeric", nullable: false),
                    Reclong = table.Column<decimal>(type: "numeric", nullable: false),
                    GeolocationType = table.Column<string>(type: "text", nullable: true),
                    Coordinates = table.Column<decimal[]>(type: "numeric[]", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NasaObjects", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NasaObjects");
        }
    }
}
