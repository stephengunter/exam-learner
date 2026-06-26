using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApplicationCore.Migrations
{
    /// <inheritdoc />
    public partial class BookParentId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Key",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "Ps",
                table: "Books");

            migrationBuilder.AddColumn<int>(
                name: "Number",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "Books",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Books_ParentId",
                table: "Books",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Books_ParentId",
                table: "Books",
                column: "ParentId",
                principalTable: "Books",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Books_ParentId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_ParentId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Books");

            migrationBuilder.AddColumn<string>(
                name: "Key",
                table: "Books",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Ps",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
