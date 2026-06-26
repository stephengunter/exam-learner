using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApplicationCore.Migrations
{
    /// <inheritdoc />
    public partial class BookChapterNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Key",
                table: "BookChapters");

            migrationBuilder.AddColumn<int>(
                name: "Number",
                table: "BookChapters",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Number",
                table: "BookChapters");

            migrationBuilder.AddColumn<string>(
                name: "Key",
                table: "BookChapters",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
