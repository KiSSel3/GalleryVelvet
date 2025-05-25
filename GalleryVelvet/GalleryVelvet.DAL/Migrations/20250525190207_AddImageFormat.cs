using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GalleryVelvet.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddImageFormat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Products_ProductEntityId",
                table: "Tags");

            migrationBuilder.DropTable(
                name: "ProductEntitySizeEntity");

            migrationBuilder.DropIndex(
                name: "IX_Tags_ProductEntityId",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "ProductEntityId",
                table: "Tags");

            migrationBuilder.AddColumn<string>(
                name: "Format",
                table: "Images",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Format",
                table: "Images");

            migrationBuilder.AddColumn<Guid>(
                name: "ProductEntityId",
                table: "Tags",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProductEntitySizeEntity",
                columns: table => new
                {
                    ProductsId = table.Column<Guid>(type: "uuid", nullable: false),
                    SizesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductEntitySizeEntity", x => new { x.ProductsId, x.SizesId });
                    table.ForeignKey(
                        name: "FK_ProductEntitySizeEntity_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductEntitySizeEntity_Sizes_SizesId",
                        column: x => x.SizesId,
                        principalTable: "Sizes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tags_ProductEntityId",
                table: "Tags",
                column: "ProductEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductEntitySizeEntity_SizesId",
                table: "ProductEntitySizeEntity",
                column: "SizesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Products_ProductEntityId",
                table: "Tags",
                column: "ProductEntityId",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
