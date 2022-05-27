using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class UpdateBdV3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MoreDetails",
                table: "Auctions");

            migrationBuilder.RenameColumn(
                name: "Tip",
                table: "Auctions",
                newName: "OtherDetails");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Auctions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Auctions");

            migrationBuilder.RenameColumn(
                name: "OtherDetails",
                table: "Auctions",
                newName: "Tip");

            migrationBuilder.AddColumn<string>(
                name: "MoreDetails",
                table: "Auctions",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
