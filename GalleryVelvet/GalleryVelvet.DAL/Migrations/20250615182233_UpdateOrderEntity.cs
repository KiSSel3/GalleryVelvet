using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GalleryVelvet.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_CartItems_CartItemEntityId",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_CartItems_CartItemEntityId",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_CartItemEntityId",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_CartItemEntityId",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "CartItemEntityId",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "CartItemEntityId",
                table: "CartItems");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Orders",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Orders",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Orders",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Orders",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Orders");

            migrationBuilder.AddColumn<Guid>(
                name: "CartItemEntityId",
                table: "OrderItems",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CartItemEntityId",
                table: "CartItems",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_CartItemEntityId",
                table: "OrderItems",
                column: "CartItemEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_CartItemEntityId",
                table: "CartItems",
                column: "CartItemEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_CartItems_CartItemEntityId",
                table: "CartItems",
                column: "CartItemEntityId",
                principalTable: "CartItems",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_CartItems_CartItemEntityId",
                table: "OrderItems",
                column: "CartItemEntityId",
                principalTable: "CartItems",
                principalColumn: "Id");
        }
    }
}
