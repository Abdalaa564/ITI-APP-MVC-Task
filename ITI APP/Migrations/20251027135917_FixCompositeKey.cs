using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITI_APP.Migrations
{
    /// <inheritdoc />
    public partial class FixCompositeKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CrsResults",
                table: "CrsResults");

            migrationBuilder.DropIndex(
                name: "IX_CrsResults_StudentId",
                table: "CrsResults");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "CrsResults");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CrsResults",
                table: "CrsResults",
                columns: new[] { "StudentId", "CrsId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CrsResults",
                table: "CrsResults");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "CrsResults",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CrsResults",
                table: "CrsResults",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CrsResults_StudentId",
                table: "CrsResults",
                column: "StudentId");
        }
    }
}
