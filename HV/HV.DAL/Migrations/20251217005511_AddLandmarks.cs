using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HV.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddLandmarks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Landmarks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    NormalizedName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Latitude = table.Column<decimal>(type: "decimal(9,6)", precision: 9, scale: 6, nullable: true),
                    Longitude = table.Column<decimal>(type: "decimal(9,6)", precision: 9, scale: 6, nullable: true),
                    FirstMentionYear = table.Column<int>(type: "int", nullable: true),
                    ProtectionStatus = table.Column<int>(type: "int", nullable: false),
                    PhysicalCondition = table.Column<int>(type: "int", nullable: false),
                    AccessibilityStatus = table.Column<int>(type: "int", nullable: false),
                    ExternalRegistryUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Landmarks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LandmarkTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    NormalizedName = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LandmarkLandmarkTag");

            migrationBuilder.DropTable(
                name: "LandmarkTags");

            migrationBuilder.DropTable(
                name: "Landmarks");
        }
    }
}
