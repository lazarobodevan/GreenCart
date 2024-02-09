using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class ProducerLatitudeAndLongitude : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttendedCities",
                table: "Producers");

            migrationBuilder.DropColumn(
                name: "OriginCity",
                table: "Producers");

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Producers",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Producers",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Producers");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Producers");

            migrationBuilder.AddColumn<string>(
                name: "AttendedCities",
                table: "Producers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OriginCity",
                table: "Producers",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
