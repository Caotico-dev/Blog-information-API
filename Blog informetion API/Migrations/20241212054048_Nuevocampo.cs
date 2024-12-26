using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog_informetion_API.Migrations
{
    /// <inheritdoc />
    public partial class Nuevocampo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Autor = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: true),
                    Fecha_de_publicacion = table.Column<DateOnly>(type: "date", nullable: true),
                    Campo = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    Url_images = table.Column<string>(type: "varchar(1)", unicode: false, maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__News__3214EC07FA2F815E", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "Titulo",
                table: "News",
                column: "Titulo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "News");
        }
    }
}
