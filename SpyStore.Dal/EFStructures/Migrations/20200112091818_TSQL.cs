using Microsoft.EntityFrameworkCore.Migrations;
using SpyStore.Dal.EFStructures.MigrationHelpers;

namespace SpyStore.Dal.EFStructures.Migrations
{
    public partial class TSQL : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            ViewsHelper.CreateOrderDetailWithProductInfoView(migrationBuilder);
            ViewsHelper.CreateRecordWithProductInfoView(migrationBuilder);
            FunctionsHelpers.CreateOrderTotalFunction(migrationBuilder);
            SprocsHelper.CreatePurchaseSproc(migrationBuilder);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            ViewsHelper.DropCartRecordWithProductInfoView(migrationBuilder);
            ViewsHelper.DropOrderDetailWithProductInfoView(migrationBuilder);
            FunctionsHelpers.DropOrderTotalFunction(migrationBuilder);
            SprocsHelper.DropPurchaseSproc(migrationBuilder);
        }
    }
}
