using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApplicationCore.Migrations
{
    /// <inheritdoc />
    public partial class UserRefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Auth.RefreshToken_AspNetUsers_UserId1",
                table: "Auth.RefreshToken");

            migrationBuilder.DropIndex(
                name: "IX_Auth.RefreshToken_UserId1",
                table: "Auth.RefreshToken");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Auth.RefreshToken");

            migrationBuilder.AddForeignKey(
                name: "FK_Auth.RefreshToken_AspNetUsers_UserId",
                table: "Auth.RefreshToken",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Auth.RefreshToken_AspNetUsers_UserId",
                table: "Auth.RefreshToken");

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "Auth.RefreshToken",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Auth.RefreshToken_UserId1",
                table: "Auth.RefreshToken",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Auth.RefreshToken_AspNetUsers_UserId1",
                table: "Auth.RefreshToken",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
