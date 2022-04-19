using Microsoft.EntityFrameworkCore.Migrations;

namespace CustomerVehicleManagement.Api.Migrations
{
    public partial class AddRepairOrderItemPurchases : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_RepairOrderPurchase_RepairOrderItemId",
                schema: "dbo",
                table: "RepairOrderPurchase",
                column: "RepairOrderItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_RepairOrderPurchase_RepairOrderItem_RepairOrderItemId",
                schema: "dbo",
                table: "RepairOrderPurchase",
                column: "RepairOrderItemId",
                principalSchema: "dbo",
                principalTable: "RepairOrderItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RepairOrderPurchase_RepairOrderItem_RepairOrderItemId",
                schema: "dbo",
                table: "RepairOrderPurchase");

            migrationBuilder.DropIndex(
                name: "IX_RepairOrderPurchase_RepairOrderItemId",
                schema: "dbo",
                table: "RepairOrderPurchase");
        }
    }
}
