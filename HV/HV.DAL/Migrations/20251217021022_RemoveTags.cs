using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HV.DAL.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LandmarkLandmarkTag");

            migrationBuilder.DropTable(
                name: "LandmarkTags");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LandmarkTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    NormalizedName = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LandmarkTags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LandmarkLandmarkTag",
                columns: table => new
                {
                    LandmarkId = table.Column<int>(type: "int", nullable: false),
                    LandmarkTagId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LandmarkLandmarkTag", x => new { x.LandmarkId, x.LandmarkTagId });
                    table.ForeignKey(
                        name: "FK_LandmarkLandmarkTag_LandmarkTags_LandmarkTagId",
                        column: x => x.LandmarkTagId,
                        principalTable: "LandmarkTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LandmarkLandmarkTag_Landmarks_LandmarkId",
                        column: x => x.LandmarkId,
                        principalTable: "Landmarks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LandmarkLandmarkTag_LandmarkTagId",
                table: "LandmarkLandmarkTag",
                column: "LandmarkTagId");

            migrationBuilder.CreateIndex(
                name: "IX_LandmarkTags_NormalizedName",
                table: "LandmarkTags",
                column: "NormalizedName",
                unique: true);
        }
    }
}
