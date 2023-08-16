using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Menominee.Api.Migrations
{
    public partial class RenameLaborAmountPayTypeToLaborAmountType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TechAmount_PayType",
                schema: "dbo",
                table: "RepairOrderItemPart",
                newName: "TechAmount_Type");

            migrationBuilder.RenameColumn(
                name: "TechAmount_PayType",
                schema: "dbo",
                table: "RepairOrderItemLabor",
                newName: "TechAmount_Type");

            migrationBuilder.RenameColumn(
                name: "LaborAmount_PayType",
                schema: "dbo",
                table: "RepairOrderItemLabor",
                newName: "LaborAmount_Type");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TechAmount_Type",
                schema: "dbo",
                table: "RepairOrderItemPart",
                newName: "TechAmount_PayType");

            migrationBuilder.RenameColumn(
                name: "TechAmount_Type",
                schema: "dbo",
                table: "RepairOrderItemLabor",
                newName: "TechAmount_PayType");

            migrationBuilder.RenameColumn(
                name: "LaborAmount_Type",
                schema: "dbo",
                table: "RepairOrderItemLabor",
                newName: "LaborAmount_PayType");
        }
    }
}
