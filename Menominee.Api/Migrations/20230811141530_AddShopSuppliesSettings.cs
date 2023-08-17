using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace Menominee.Api.Migrations
{
    public partial class AddShopSuppliesSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Setting",
                schema: "dbo",
                columns: new[]
                {
                    "SettingName",
                    "SettingGroup",
                    "SettingValueType",
                    "Value"
                },
                values: new object[,]
                {
                    //SettingName.DisplayName = 3, SettingGroup.ShopSupplies = 4
                    { 3, 4, nameof(String), "Shop Supplies" },
                    //SettingName.ReportInSaleCode = 4, SettingGroup.ShopSupplies = 4
                    { 4, 4, nameof(Int64), 0L.ToString() },
                    //SettingName.MaximumCharge = 5, SettingGroup.ShopSupplies = 4
                    { 5, 4, nameof(Decimal), 0M.ToString() },
                    //SettingName.CostType = 6, SettingGroup.ShopSupplies = 4,
                    //ShopSuppliesCostType.None = 0
                    { 6, 4, nameof(Int32), 0.ToString() },
                    //SettingName.CostPerInvoice = 7, SettingGroup.ShopSupplies = 4
                    { 7, 4, nameof(Decimal), 0M.ToString() },
                });
        }
    }
}
