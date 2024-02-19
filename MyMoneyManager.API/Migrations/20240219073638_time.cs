using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyMoneyManager.API.Migrations
{
    /// <inheritdoc />
    public partial class time : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Tag_TagId",
                table: "Transaction");

            migrationBuilder.AlterColumn<int>(
                name: "TagId",
                table: "Transaction",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Tag_TagId",
                table: "Transaction",
                column: "TagId",
                principalTable: "Tag",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Tag_TagId",
                table: "Transaction");

            migrationBuilder.AlterColumn<int>(
                name: "TagId",
                table: "Transaction",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Tag_TagId",
                table: "Transaction",
                column: "TagId",
                principalTable: "Tag",
                principalColumn: "Id");
        }
    }
}
