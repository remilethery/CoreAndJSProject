using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SpyStore.Dal.EFStructures.MigrationHelpers
{

    public static class ViewsHelper
    {
        public static void CreateOrderDetailWithProductInfoView(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE VIEW [Store].[OrderDetailWithProductInfo] AS
                    SELECT od.Id, od.TimeStamp, od.OrderId, od.ProductId,
                        od.Quantity, od.UnitCost,
                        od.Quantity * od.UnitCost AS LineItemTotal,
                        p.ModelName, p.Description, p.ModelNumber, p.ProductImage,
                        p.ProductImageLarge, p.ProductImageThumb, p.CategoryId,
                        p.UnitsInStock, p.CurrentPrise, c.CategoryName
                    FROM Store.OrderDetails od
                        INNER JOIN Store.Orders o ON o.Id = od.OrderId
                        INNER JOIN Store.Products AS p ON  od.ProductId = p.Id
                        INNER JOIN Store.Categories AS c ON p.CategoryId = c.Id
            ");
        }

        public static void CreateRecordWithProductInfoView(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE VIEW [Store].[OrderDetailWithProductInfo] AS
                    SELECT scr.Id, scr.TimeStamp, scr.DateCreated, scr.CustomerId,
                        scr.Quantity, scr.LineItemTotal, scr.ProductId, p.ModelName,
                        p.Description, p.ModelNumber, p.ProductImage,
                        p.ProductImageLarge, p.ProductImageThumb, p.CategoryId,
                        p.UnitsInStock, p.CurrentPrice, c.CategoryName
                    FROM Store.ShoppingCartRecords scr
                        INNER JOIN Store.Products p ON p.Id = scr.ProductId
                        INNER JOIN Store.Categories c ON c.Id = p.CategoryId
            ");
        }
        public static void DropOrderDetailWithProductInfoView(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("drop view [Store].[OrderDetailWithProductInfo]");
        }
        public static void DropCartRecordWithProductInfoView(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("drop view [Store].[CartRecordWithProductInfo]");
        }
    }
}