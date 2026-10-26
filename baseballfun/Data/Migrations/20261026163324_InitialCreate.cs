using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace baseballfun.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Opponent = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsHome = table.Column<bool>(type: "bit", nullable: false),
                    BrewersRuns = table.Column<int>(type: "int", nullable: true),
                    OpponentRuns = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Position = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Bats = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Throws = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    BirthDate = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlayerStats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlayerId = table.Column<int>(type: "int", nullable: false),
                    Season = table.Column<int>(type: "int", nullable: false),
                    AtBats = table.Column<int>(type: "int", nullable: true),
                    Hits = table.Column<int>(type: "int", nullable: true),
                    HomeRuns = table.Column<int>(type: "int", nullable: true),
                    RunsBattedIn = table.Column<int>(type: "int", nullable: true),
                    StolenBases = table.Column<int>(type: "int", nullable: true),
                    InningsPitched = table.Column<decimal>(type: "decimal(5,1)", precision: 5, scale: 1, nullable: true),
                    Wins = table.Column<int>(type: "int", nullable: true),
                    Losses = table.Column<int>(type: "int", nullable: true),
                    Saves = table.Column<int>(type: "int", nullable: true),
                    Strikeouts = table.Column<int>(type: "int", nullable: true),
                    EarnedRuns = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerStats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerStats_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Games_GameDate",
                table: "Games",
                column: "GameDate");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerStats_PlayerId_Season",
                table: "PlayerStats",
                columns: new[] { "PlayerId", "Season" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "PlayerStats");

            migrationBuilder.DropTable(
                name: "Players");
        }
    }
}
