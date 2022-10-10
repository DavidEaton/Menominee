using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerVehicleManagement.Api.Migrations
{
    public partial class InventoryMaintenanceItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItem_Manufacturer_ManufacturerId",
                schema: "dbo",
                table: "InventoryItem");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItem_ProductCode_ProductCodeId",
                schema: "dbo",
                table: "InventoryItem");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItemInspection_InventoryItem_InventoryItemId",
                schema: "dbo",
                table: "InventoryItemInspection");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItemLabor_InventoryItem_InventoryItemId",
                schema: "dbo",
                table: "InventoryItemLabor");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItemPackage_InventoryItem_InventoryItemId",
                schema: "dbo",
                table: "InventoryItemPackage");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItemPackageItem_InventoryItemPackage_InventoryItemPackageId",
                schema: "dbo",
                table: "InventoryItemPackageItem");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItemPackagePlaceholder_InventoryItemPackage_InventoryItemPackageId",
                schema: "dbo",
                table: "InventoryItemPackagePlaceholder");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItemPart_InventoryItem_InventoryItemId",
                schema: "dbo",
                table: "InventoryItemPart");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItemTire_InventoryItem_InventoryItemId",
                schema: "dbo",
                table: "InventoryItemTire");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItemWarranties_InventoryItem_InventoryItemId",
                table: "InventoryItemWarranties");

            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceItem_InventoryItem_ItemId",
                schema: "dbo",
                table: "MaintenanceItem");

            migrationBuilder.DropTable(
                name: "InventoryItemDonation");

            migrationBuilder.DropTable(
                name: "InventoryItemGiftCertificate");

            migrationBuilder.DropIndex(
                name: "IX_InventoryItemTire_InventoryItemId",
                schema: "dbo",
                table: "InventoryItemTire");

            migrationBuilder.DropIndex(
                name: "IX_InventoryItemPart_InventoryItemId",
                schema: "dbo",
                table: "InventoryItemPart");

            migrationBuilder.DropIndex(
                name: "IX_InventoryItemPackage_InventoryItemId",
                schema: "dbo",
                table: "InventoryItemPackage");

            migrationBuilder.DropIndex(
                name: "IX_InventoryItemLabor_InventoryItemId",
                schema: "dbo",
                table: "InventoryItemLabor");

            migrationBuilder.DropIndex(
                name: "IX_InventoryItemInspection_InventoryItemId",
                schema: "dbo",
                table: "InventoryItemInspection");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InventoryItemWarranties",
                table: "InventoryItemWarranties");

            migrationBuilder.DropIndex(
                name: "IX_InventoryItemWarranties_InventoryItemId",
                table: "InventoryItemWarranties");

            migrationBuilder.DropColumn(
                name: "InventoryItemId",
                schema: "dbo",
                table: "InventoryItemTire");

            migrationBuilder.DropColumn(
                name: "InventoryItemId",
                schema: "dbo",
                table: "InventoryItemPart");

            migrationBuilder.DropColumn(
                name: "InventoryItemId",
                schema: "dbo",
                table: "InventoryItemPackage");

            migrationBuilder.DropColumn(
                name: "InventoryItemId",
                schema: "dbo",
                table: "InventoryItemLabor");

            migrationBuilder.DropColumn(
                name: "LaborAmount",
                schema: "dbo",
                table: "InventoryItemLabor");

            migrationBuilder.DropColumn(
                name: "LaborType",
                schema: "dbo",
                table: "InventoryItemLabor");

            migrationBuilder.DropColumn(
                name: "TechPayAmount",
                schema: "dbo",
                table: "InventoryItemLabor");

            migrationBuilder.DropColumn(
                name: "InventoryItemId",
                schema: "dbo",
                table: "InventoryItemInspection");

            migrationBuilder.DropColumn(
                name: "LaborAmount",
                schema: "dbo",
                table: "InventoryItemInspection");

            migrationBuilder.DropColumn(
                name: "LaborType",
                schema: "dbo",
                table: "InventoryItemInspection");

            migrationBuilder.DropColumn(
                name: "TechPayAmount",
                schema: "dbo",
                table: "InventoryItemInspection");

            migrationBuilder.DropColumn(
                name: "InventoryItemId",
                table: "InventoryItemWarranties");

            migrationBuilder.RenameTable(
                name: "InventoryItemWarranties",
                newName: "InventoryItemWarranty",
                newSchema: "dbo");

            migrationBuilder.RenameColumn(
                name: "ItemId",
                schema: "dbo",
                table: "MaintenanceItem",
                newName: "InventoryItemId");

            migrationBuilder.RenameIndex(
                name: "IX_MaintenanceItem_ItemId",
                schema: "dbo",
                table: "MaintenanceItem",
                newName: "IX_MaintenanceItem_InventoryItemId");

            migrationBuilder.RenameColumn(
                name: "SkillLevel",
                schema: "dbo",
                table: "InventoryItemTire",
                newName: "TechSkillLevel");

            migrationBuilder.RenameColumn(
                name: "SkillLevel",
                schema: "dbo",
                table: "InventoryItemPart",
                newName: "TechSkillLevel");

            migrationBuilder.RenameColumn(
                name: "Order",
                schema: "dbo",
                table: "InventoryItemPackagePlaceholder",
                newName: "DisplayOrder");

            migrationBuilder.RenameColumn(
                name: "Order",
                schema: "dbo",
                table: "InventoryItemPackageItem",
                newName: "DisplayOrder");

            migrationBuilder.RenameColumn(
                name: "SkillLevel",
                schema: "dbo",
                table: "InventoryItemLabor",
                newName: "TechSkillLevel");

            migrationBuilder.RenameColumn(
                name: "SkillLevel",
                schema: "dbo",
                table: "InventoryItemInspection",
                newName: "TechSkillLevel");

            migrationBuilder.RenameColumn(
                name: "Type",
                schema: "dbo",
                table: "InventoryItemInspection",
                newName: "InspectionType");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "dbo",
                table: "Manufacturer",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                schema: "dbo",
                table: "Manufacturer",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "DisplayOrder",
                schema: "dbo",
                table: "MaintenanceItem",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "Width",
                schema: "dbo",
                table: "InventoryItemTire",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                schema: "dbo",
                table: "InventoryItemTire",
                type: "nvarchar(2)",
                maxLength: 2,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TechPayType",
                schema: "dbo",
                table: "InventoryItemTire",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "TechPayAmount",
                schema: "dbo",
                table: "InventoryItemTire",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<string>(
                name: "SubLineCode",
                schema: "dbo",
                table: "InventoryItemTire",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LineCode",
                schema: "dbo",
                table: "InventoryItemTire",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AspectRatio",
                schema: "dbo",
                table: "InventoryItemTire",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<int>(
                name: "TechSkillLevel",
                schema: "dbo",
                table: "InventoryItemTire",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ConstructionType",
                schema: "dbo",
                table: "InventoryItemTire",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "TechPayType",
                schema: "dbo",
                table: "InventoryItemPart",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "TechPayAmount",
                schema: "dbo",
                table: "InventoryItemPart",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<string>(
                name: "SubLineCode",
                schema: "dbo",
                table: "InventoryItemPart",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LineCode",
                schema: "dbo",
                table: "InventoryItemPart",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TechSkillLevel",
                schema: "dbo",
                table: "InventoryItemPart",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "Quantity",
                schema: "dbo",
                table: "InventoryItemPackagePlaceholder",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<bool>(
                name: "PartAmountIsAdditional",
                schema: "dbo",
                table: "InventoryItemPackagePlaceholder",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "LaborAmountIsAdditional",
                schema: "dbo",
                table: "InventoryItemPackagePlaceholder",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<long>(
                name: "InventoryItemPackageId",
                schema: "dbo",
                table: "InventoryItemPackagePlaceholder",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<bool>(
                name: "ExciseFeeIsAdditional",
                schema: "dbo",
                table: "InventoryItemPackagePlaceholder",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "dbo",
                table: "InventoryItemPackagePlaceholder",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Quantity",
                schema: "dbo",
                table: "InventoryItemPackageItem",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<bool>(
                name: "PartAmountIsAdditional",
                schema: "dbo",
                table: "InventoryItemPackageItem",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "LaborAmountIsAdditional",
                schema: "dbo",
                table: "InventoryItemPackageItem",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<long>(
                name: "InventoryItemPackageId",
                schema: "dbo",
                table: "InventoryItemPackageItem",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<bool>(
                name: "ExciseFeeIsAdditional",
                schema: "dbo",
                table: "InventoryItemPackageItem",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<int>(
                name: "TechPayType",
                schema: "dbo",
                table: "InventoryItemLabor",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "TechSkillLevel",
                schema: "dbo",
                table: "InventoryItemLabor",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<double>(
                name: "LaborPayAmount",
                schema: "dbo",
                table: "InventoryItemLabor",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LaborPayType",
                schema: "dbo",
                table: "InventoryItemLabor",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TechAmount_Amount",
                schema: "dbo",
                table: "InventoryItemLabor",
                type: "float",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TechPayType",
                schema: "dbo",
                table: "InventoryItemInspection",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "TechSkillLevel",
                schema: "dbo",
                table: "InventoryItemInspection",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<double>(
                name: "LaborPayAmount",
                schema: "dbo",
                table: "InventoryItemInspection",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LaborPayType",
                schema: "dbo",
                table: "InventoryItemInspection",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TechAmount_Amount",
                schema: "dbo",
                table: "InventoryItemInspection",
                type: "float",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ProductCodeId",
                schema: "dbo",
                table: "InventoryItem",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "ManufacturerId",
                schema: "dbo",
                table: "InventoryItem",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "ItemNumber",
                schema: "dbo",
                table: "InventoryItem",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "dbo",
                table: "InventoryItem",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<long>(
                name: "InspectionId",
                schema: "dbo",
                table: "InventoryItem",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LaborId",
                schema: "dbo",
                table: "InventoryItem",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PackageId",
                schema: "dbo",
                table: "InventoryItem",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PartId",
                schema: "dbo",
                table: "InventoryItem",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TireId",
                schema: "dbo",
                table: "InventoryItem",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "WarrantyId",
                schema: "dbo",
                table: "InventoryItem",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PeriodType",
                schema: "dbo",
                table: "InventoryItemWarranty",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Duration",
                schema: "dbo",
                table: "InventoryItemWarranty",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InventoryItemWarranty",
                schema: "dbo",
                table: "InventoryItemWarranty",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItem_InspectionId",
                schema: "dbo",
                table: "InventoryItem",
                column: "InspectionId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItem_LaborId",
                schema: "dbo",
                table: "InventoryItem",
                column: "LaborId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItem_PackageId",
                schema: "dbo",
                table: "InventoryItem",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItem_PartId",
                schema: "dbo",
                table: "InventoryItem",
                column: "PartId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItem_TireId",
                schema: "dbo",
                table: "InventoryItem",
                column: "TireId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItem_WarrantyId",
                schema: "dbo",
                table: "InventoryItem",
                column: "WarrantyId");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItem_InventoryItemInspection_InspectionId",
                schema: "dbo",
                table: "InventoryItem",
                column: "InspectionId",
                principalSchema: "dbo",
                principalTable: "InventoryItemInspection",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItem_InventoryItemLabor_LaborId",
                schema: "dbo",
                table: "InventoryItem",
                column: "LaborId",
                principalSchema: "dbo",
                principalTable: "InventoryItemLabor",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItem_InventoryItemPackage_PackageId",
                schema: "dbo",
                table: "InventoryItem",
                column: "PackageId",
                principalSchema: "dbo",
                principalTable: "InventoryItemPackage",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItem_InventoryItemPart_PartId",
                schema: "dbo",
                table: "InventoryItem",
                column: "PartId",
                principalSchema: "dbo",
                principalTable: "InventoryItemPart",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItem_InventoryItemTire_TireId",
                schema: "dbo",
                table: "InventoryItem",
                column: "TireId",
                principalSchema: "dbo",
                principalTable: "InventoryItemTire",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItem_InventoryItemWarranty_WarrantyId",
                schema: "dbo",
                table: "InventoryItem",
                column: "WarrantyId",
                principalSchema: "dbo",
                principalTable: "InventoryItemWarranty",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItem_Manufacturer_ManufacturerId",
                schema: "dbo",
                table: "InventoryItem",
                column: "ManufacturerId",
                principalSchema: "dbo",
                principalTable: "Manufacturer",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItem_ProductCode_ProductCodeId",
                schema: "dbo",
                table: "InventoryItem",
                column: "ProductCodeId",
                principalSchema: "dbo",
                principalTable: "ProductCode",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItemPackageItem_InventoryItemPackage_InventoryItemPackageId",
                schema: "dbo",
                table: "InventoryItemPackageItem",
                column: "InventoryItemPackageId",
                principalSchema: "dbo",
                principalTable: "InventoryItemPackage",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItemPackagePlaceholder_InventoryItemPackage_InventoryItemPackageId",
                schema: "dbo",
                table: "InventoryItemPackagePlaceholder",
                column: "InventoryItemPackageId",
                principalSchema: "dbo",
                principalTable: "InventoryItemPackage",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MaintenanceItem_InventoryItem_InventoryItemId",
                schema: "dbo",
                table: "MaintenanceItem",
                column: "InventoryItemId",
                principalSchema: "dbo",
                principalTable: "InventoryItem",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItem_InventoryItemInspection_InspectionId",
                schema: "dbo",
                table: "InventoryItem");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItem_InventoryItemLabor_LaborId",
                schema: "dbo",
                table: "InventoryItem");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItem_InventoryItemPackage_PackageId",
                schema: "dbo",
                table: "InventoryItem");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItem_InventoryItemPart_PartId",
                schema: "dbo",
                table: "InventoryItem");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItem_InventoryItemTire_TireId",
                schema: "dbo",
                table: "InventoryItem");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItem_InventoryItemWarranty_WarrantyId",
                schema: "dbo",
                table: "InventoryItem");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItem_Manufacturer_ManufacturerId",
                schema: "dbo",
                table: "InventoryItem");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItem_ProductCode_ProductCodeId",
                schema: "dbo",
                table: "InventoryItem");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItemPackageItem_InventoryItemPackage_InventoryItemPackageId",
                schema: "dbo",
                table: "InventoryItemPackageItem");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItemPackagePlaceholder_InventoryItemPackage_InventoryItemPackageId",
                schema: "dbo",
                table: "InventoryItemPackagePlaceholder");

            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceItem_InventoryItem_InventoryItemId",
                schema: "dbo",
                table: "MaintenanceItem");

            migrationBuilder.DropIndex(
                name: "IX_InventoryItem_InspectionId",
                schema: "dbo",
                table: "InventoryItem");

            migrationBuilder.DropIndex(
                name: "IX_InventoryItem_LaborId",
                schema: "dbo",
                table: "InventoryItem");

            migrationBuilder.DropIndex(
                name: "IX_InventoryItem_PackageId",
                schema: "dbo",
                table: "InventoryItem");

            migrationBuilder.DropIndex(
                name: "IX_InventoryItem_PartId",
                schema: "dbo",
                table: "InventoryItem");

            migrationBuilder.DropIndex(
                name: "IX_InventoryItem_TireId",
                schema: "dbo",
                table: "InventoryItem");

            migrationBuilder.DropIndex(
                name: "IX_InventoryItem_WarrantyId",
                schema: "dbo",
                table: "InventoryItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InventoryItemWarranty",
                schema: "dbo",
                table: "InventoryItemWarranty");

            migrationBuilder.DropColumn(
                name: "ConstructionType",
                schema: "dbo",
                table: "InventoryItemTire");

            migrationBuilder.DropColumn(
                name: "LaborPayAmount",
                schema: "dbo",
                table: "InventoryItemLabor");

            migrationBuilder.DropColumn(
                name: "LaborPayType",
                schema: "dbo",
                table: "InventoryItemLabor");

            migrationBuilder.DropColumn(
                name: "TechAmount_Amount",
                schema: "dbo",
                table: "InventoryItemLabor");

            migrationBuilder.DropColumn(
                name: "LaborPayAmount",
                schema: "dbo",
                table: "InventoryItemInspection");

            migrationBuilder.DropColumn(
                name: "LaborPayType",
                schema: "dbo",
                table: "InventoryItemInspection");

            migrationBuilder.DropColumn(
                name: "TechAmount_Amount",
                schema: "dbo",
                table: "InventoryItemInspection");

            migrationBuilder.DropColumn(
                name: "InspectionId",
                schema: "dbo",
                table: "InventoryItem");

            migrationBuilder.DropColumn(
                name: "LaborId",
                schema: "dbo",
                table: "InventoryItem");

            migrationBuilder.DropColumn(
                name: "PackageId",
                schema: "dbo",
                table: "InventoryItem");

            migrationBuilder.DropColumn(
                name: "PartId",
                schema: "dbo",
                table: "InventoryItem");

            migrationBuilder.DropColumn(
                name: "TireId",
                schema: "dbo",
                table: "InventoryItem");

            migrationBuilder.DropColumn(
                name: "WarrantyId",
                schema: "dbo",
                table: "InventoryItem");

            migrationBuilder.RenameTable(
                name: "InventoryItemWarranty",
                schema: "dbo",
                newName: "InventoryItemWarranties");

            migrationBuilder.RenameColumn(
                name: "InventoryItemId",
                schema: "dbo",
                table: "MaintenanceItem",
                newName: "ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_MaintenanceItem_InventoryItemId",
                schema: "dbo",
                table: "MaintenanceItem",
                newName: "IX_MaintenanceItem_ItemId");

            migrationBuilder.RenameColumn(
                name: "TechSkillLevel",
                schema: "dbo",
                table: "InventoryItemTire",
                newName: "SkillLevel");

            migrationBuilder.RenameColumn(
                name: "TechSkillLevel",
                schema: "dbo",
                table: "InventoryItemPart",
                newName: "SkillLevel");

            migrationBuilder.RenameColumn(
                name: "DisplayOrder",
                schema: "dbo",
                table: "InventoryItemPackagePlaceholder",
                newName: "Order");

            migrationBuilder.RenameColumn(
                name: "DisplayOrder",
                schema: "dbo",
                table: "InventoryItemPackageItem",
                newName: "Order");

            migrationBuilder.RenameColumn(
                name: "TechSkillLevel",
                schema: "dbo",
                table: "InventoryItemLabor",
                newName: "SkillLevel");

            migrationBuilder.RenameColumn(
                name: "TechSkillLevel",
                schema: "dbo",
                table: "InventoryItemInspection",
                newName: "SkillLevel");

            migrationBuilder.RenameColumn(
                name: "InspectionType",
                schema: "dbo",
                table: "InventoryItemInspection",
                newName: "Type");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "dbo",
                table: "Manufacturer",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                schema: "dbo",
                table: "Manufacturer",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<long>(
                name: "DisplayOrder",
                schema: "dbo",
                table: "MaintenanceItem",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "Width",
                schema: "dbo",
                table: "InventoryItemTire",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                schema: "dbo",
                table: "InventoryItemTire",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2)",
                oldMaxLength: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TechPayType",
                schema: "dbo",
                table: "InventoryItemTire",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "TechPayAmount",
                schema: "dbo",
                table: "InventoryItemTire",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SubLineCode",
                schema: "dbo",
                table: "InventoryItemTire",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LineCode",
                schema: "dbo",
                table: "InventoryItemTire",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "AspectRatio",
                schema: "dbo",
                table: "InventoryItemTire",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "SkillLevel",
                schema: "dbo",
                table: "InventoryItemTire",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "InventoryItemId",
                schema: "dbo",
                table: "InventoryItemTire",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<int>(
                name: "TechPayType",
                schema: "dbo",
                table: "InventoryItemPart",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "TechPayAmount",
                schema: "dbo",
                table: "InventoryItemPart",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SubLineCode",
                schema: "dbo",
                table: "InventoryItemPart",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LineCode",
                schema: "dbo",
                table: "InventoryItemPart",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SkillLevel",
                schema: "dbo",
                table: "InventoryItemPart",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "InventoryItemId",
                schema: "dbo",
                table: "InventoryItemPart",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<double>(
                name: "Quantity",
                schema: "dbo",
                table: "InventoryItemPackagePlaceholder",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "PartAmountIsAdditional",
                schema: "dbo",
                table: "InventoryItemPackagePlaceholder",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "LaborAmountIsAdditional",
                schema: "dbo",
                table: "InventoryItemPackagePlaceholder",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "InventoryItemPackageId",
                schema: "dbo",
                table: "InventoryItemPackagePlaceholder",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "ExciseFeeIsAdditional",
                schema: "dbo",
                table: "InventoryItemPackagePlaceholder",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "dbo",
                table: "InventoryItemPackagePlaceholder",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<double>(
                name: "Quantity",
                schema: "dbo",
                table: "InventoryItemPackageItem",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "PartAmountIsAdditional",
                schema: "dbo",
                table: "InventoryItemPackageItem",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "LaborAmountIsAdditional",
                schema: "dbo",
                table: "InventoryItemPackageItem",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "InventoryItemPackageId",
                schema: "dbo",
                table: "InventoryItemPackageItem",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "ExciseFeeIsAdditional",
                schema: "dbo",
                table: "InventoryItemPackageItem",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "InventoryItemId",
                schema: "dbo",
                table: "InventoryItemPackage",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<int>(
                name: "TechPayType",
                schema: "dbo",
                table: "InventoryItemLabor",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SkillLevel",
                schema: "dbo",
                table: "InventoryItemLabor",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "InventoryItemId",
                schema: "dbo",
                table: "InventoryItemLabor",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<double>(
                name: "LaborAmount",
                schema: "dbo",
                table: "InventoryItemLabor",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "LaborType",
                schema: "dbo",
                table: "InventoryItemLabor",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "TechPayAmount",
                schema: "dbo",
                table: "InventoryItemLabor",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<int>(
                name: "TechPayType",
                schema: "dbo",
                table: "InventoryItemInspection",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SkillLevel",
                schema: "dbo",
                table: "InventoryItemInspection",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "InventoryItemId",
                schema: "dbo",
                table: "InventoryItemInspection",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<double>(
                name: "LaborAmount",
                schema: "dbo",
                table: "InventoryItemInspection",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "LaborType",
                schema: "dbo",
                table: "InventoryItemInspection",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "TechPayAmount",
                schema: "dbo",
                table: "InventoryItemInspection",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<long>(
                name: "ProductCodeId",
                schema: "dbo",
                table: "InventoryItem",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ManufacturerId",
                schema: "dbo",
                table: "InventoryItem",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ItemNumber",
                schema: "dbo",
                table: "InventoryItem",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "dbo",
                table: "InventoryItem",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PeriodType",
                table: "InventoryItemWarranties",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Duration",
                table: "InventoryItemWarranties",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "InventoryItemId",
                table: "InventoryItemWarranties",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddPrimaryKey(
                name: "PK_InventoryItemWarranties",
                table: "InventoryItemWarranties",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "InventoryItemDonation",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InventoryItemId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryItemDonation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryItemDonation_InventoryItem_InventoryItemId",
                        column: x => x.InventoryItemId,
                        principalSchema: "dbo",
                        principalTable: "InventoryItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InventoryItemGiftCertificate",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InventoryItemId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryItemGiftCertificate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryItemGiftCertificate_InventoryItem_InventoryItemId",
                        column: x => x.InventoryItemId,
                        principalSchema: "dbo",
                        principalTable: "InventoryItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItemTire_InventoryItemId",
                schema: "dbo",
                table: "InventoryItemTire",
                column: "InventoryItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItemPart_InventoryItemId",
                schema: "dbo",
                table: "InventoryItemPart",
                column: "InventoryItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItemPackage_InventoryItemId",
                schema: "dbo",
                table: "InventoryItemPackage",
                column: "InventoryItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItemLabor_InventoryItemId",
                schema: "dbo",
                table: "InventoryItemLabor",
                column: "InventoryItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItemInspection_InventoryItemId",
                schema: "dbo",
                table: "InventoryItemInspection",
                column: "InventoryItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItemWarranties_InventoryItemId",
                table: "InventoryItemWarranties",
                column: "InventoryItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItemDonation_InventoryItemId",
                table: "InventoryItemDonation",
                column: "InventoryItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItemGiftCertificate_InventoryItemId",
                table: "InventoryItemGiftCertificate",
                column: "InventoryItemId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItem_Manufacturer_ManufacturerId",
                schema: "dbo",
                table: "InventoryItem",
                column: "ManufacturerId",
                principalSchema: "dbo",
                principalTable: "Manufacturer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItem_ProductCode_ProductCodeId",
                schema: "dbo",
                table: "InventoryItem",
                column: "ProductCodeId",
                principalSchema: "dbo",
                principalTable: "ProductCode",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItemInspection_InventoryItem_InventoryItemId",
                schema: "dbo",
                table: "InventoryItemInspection",
                column: "InventoryItemId",
                principalSchema: "dbo",
                principalTable: "InventoryItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItemLabor_InventoryItem_InventoryItemId",
                schema: "dbo",
                table: "InventoryItemLabor",
                column: "InventoryItemId",
                principalSchema: "dbo",
                principalTable: "InventoryItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItemPackage_InventoryItem_InventoryItemId",
                schema: "dbo",
                table: "InventoryItemPackage",
                column: "InventoryItemId",
                principalSchema: "dbo",
                principalTable: "InventoryItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItemPackageItem_InventoryItemPackage_InventoryItemPackageId",
                schema: "dbo",
                table: "InventoryItemPackageItem",
                column: "InventoryItemPackageId",
                principalSchema: "dbo",
                principalTable: "InventoryItemPackage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItemPackagePlaceholder_InventoryItemPackage_InventoryItemPackageId",
                schema: "dbo",
                table: "InventoryItemPackagePlaceholder",
                column: "InventoryItemPackageId",
                principalSchema: "dbo",
                principalTable: "InventoryItemPackage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItemPart_InventoryItem_InventoryItemId",
                schema: "dbo",
                table: "InventoryItemPart",
                column: "InventoryItemId",
                principalSchema: "dbo",
                principalTable: "InventoryItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItemTire_InventoryItem_InventoryItemId",
                schema: "dbo",
                table: "InventoryItemTire",
                column: "InventoryItemId",
                principalSchema: "dbo",
                principalTable: "InventoryItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItemWarranties_InventoryItem_InventoryItemId",
                table: "InventoryItemWarranties",
                column: "InventoryItemId",
                principalSchema: "dbo",
                principalTable: "InventoryItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MaintenanceItem_InventoryItem_ItemId",
                schema: "dbo",
                table: "MaintenanceItem",
                column: "ItemId",
                principalSchema: "dbo",
                principalTable: "InventoryItem",
                principalColumn: "Id");
        }
    }
}
