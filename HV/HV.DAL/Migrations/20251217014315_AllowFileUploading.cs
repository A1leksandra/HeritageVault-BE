using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HV.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AllowFileUploading : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Landmarks",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UploadedImagePath",
                table: "Landmarks",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Landmarks");

            migrationBuilder.DropColumn(
                name: "UploadedImagePath",
                table: "Landmarks");
        }
    }
}
