using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class relationsfavorite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Producers_Consumers_ConsumerId",
                table: "Producers");

            migrationBuilder.DropIndex(
                name: "IX_Producers_ConsumerId",
                table: "Producers");

            migrationBuilder.DropColumn(
                name: "ConsumerId",
                table: "Producers");

            migrationBuilder.CreateTable(
                name: "ConsumerFavProducer",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    ConsumerId = table.Column<string>(type: "text", nullable: false),
                    ProducerId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsumerFavProducer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConsumerFavProducer_Consumers_ConsumerId",
                        column: x => x.ConsumerId,
                        principalTable: "Consumers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConsumerFavProducer_Producers_ProducerId",
                        column: x => x.ProducerId,
                        principalTable: "Producers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConsumerFavProducer_ConsumerId",
                table: "ConsumerFavProducer",
                column: "ConsumerId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsumerFavProducer_ProducerId",
                table: "ConsumerFavProducer",
                column: "ProducerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConsumerFavProducer");

            migrationBuilder.AddColumn<string>(
                name: "ConsumerId",
                table: "Producers",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Producers_ConsumerId",
                table: "Producers",
                column: "ConsumerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Producers_Consumers_ConsumerId",
                table: "Producers",
                column: "ConsumerId",
                principalTable: "Consumers",
                principalColumn: "Id");
        }
    }
}
