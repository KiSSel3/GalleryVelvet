using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GalleryVelvet.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddCommentsInOrderEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comments",
                table: "Orders",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comments",
                table: "Orders");
        }
    }
}
