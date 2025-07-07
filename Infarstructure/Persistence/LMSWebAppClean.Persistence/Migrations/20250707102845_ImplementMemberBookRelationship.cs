using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMSWebAppClean.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ImplementMemberBookRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Available",
                table: "Books",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.CreateIndex(
                name: "IX_Books_Available",
                table: "Books",
                column: "Available");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Books_Available",
                table: "Books");

            migrationBuilder.AlterColumn<bool>(
                name: "Available",
                table: "Books",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);
        }
    }
}
