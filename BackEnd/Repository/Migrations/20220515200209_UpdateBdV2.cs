using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class UpdateBdV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Images_AuctionId",
                table: "Images",
                column: "AuctionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Auctions_AuctionId",
                table: "Images",
                column: "AuctionId",
                principalTable: "Auctions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Auctions_AuctionId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_AuctionId",
                table: "Images");
        }
    }
}
