using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoHub.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedFavouriteCars : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CarUser",
                columns: table => new
                {
                    FavouriteCarsId = table.Column<Guid>(type: "uuid", nullable: false),
                    UsersFavouriteId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarUser", x => new { x.FavouriteCarsId, x.UsersFavouriteId });
                    table.ForeignKey(
                        name: "FK_CarUser_AspNetUsers_UsersFavouriteId",
                        column: x => x.UsersFavouriteId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CarUser_Cars_FavouriteCarsId",
                        column: x => x.FavouriteCarsId,
                        principalTable: "Cars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CarUser_UsersFavouriteId",
                table: "CarUser",
                column: "UsersFavouriteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarUser");
        }
    }
}
