using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoHub.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixedTheRelationsBetweenImageAndCar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Image_Cars_CarId",
                table: "Image");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Image",
                table: "Image");

            migrationBuilder.RenameTable(
                name: "Image",
                newName: "Images");

            migrationBuilder.RenameColumn(
                name: "url",
                table: "Images",
                newName: "Url");

            migrationBuilder.RenameIndex(
                name: "IX_Image_CarId",
                table: "Images",
                newName: "IX_Images_CarId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Images",
                table: "Images",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Cars_CarId",
                table: "Images",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Cars_CarId",
                table: "Images");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Images",
                table: "Images");

            migrationBuilder.RenameTable(
                name: "Images",
                newName: "Image");

            migrationBuilder.RenameColumn(
                name: "Url",
                table: "Image",
                newName: "url");

            migrationBuilder.RenameIndex(
                name: "IX_Images_CarId",
                table: "Image",
                newName: "IX_Image_CarId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Image",
                table: "Image",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Image_Cars_CarId",
                table: "Image",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
