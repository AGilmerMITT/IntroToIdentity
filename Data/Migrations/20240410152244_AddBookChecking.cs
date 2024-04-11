using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IntroToIdentity.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddBookChecking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CurrentApplicationUserId",
                table: "Books",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Books_CurrentApplicationUserId",
                table: "Books",
                column: "CurrentApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_AspNetUsers_CurrentApplicationUserId",
                table: "Books",
                column: "CurrentApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_AspNetUsers_CurrentApplicationUserId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_CurrentApplicationUserId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "CurrentApplicationUserId",
                table: "Books");
        }
    }
}
