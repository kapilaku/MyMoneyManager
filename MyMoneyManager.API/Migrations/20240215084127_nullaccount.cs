using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyMoneyManager.API.Migrations
{
    /// <inheritdoc />
    public partial class nullaccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Account_Account_ParentAccountId",
                table: "Account");

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

            migrationBuilder.AlterColumn<int>(
                name: "ParentAccountId",
                table: "Account",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Account_Account_ParentAccountId",
                table: "Account",
                column: "ParentAccountId",
                principalTable: "Account",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Tag_TagId",
                table: "Transaction",
                column: "TagId",
                principalTable: "Tag",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Account_Account_ParentAccountId",
                table: "Account");

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

            migrationBuilder.AlterColumn<int>(
                name: "ParentAccountId",
                table: "Account",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Account_Account_ParentAccountId",
                table: "Account",
                column: "ParentAccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Tag_TagId",
                table: "Transaction",
                column: "TagId",
                principalTable: "Tag",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
