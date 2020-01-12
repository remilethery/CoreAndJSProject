using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SpyStore.Dal.EFStructures.MigrationHelpers
{
    public class FunctionsHelpers
    {
        public static void CreateOrderTotalFunction(MigrationBuilder migrationBuilder)
        {
            string sql = @"
                CREATE FUNCTION Store.GetOrderTotal (@OrderId Int)
                RETURNS MONEY WITH SCHEMABINDING
                BEGIN
                    DECLARE @Result MONEY;
                    SELECT @Result = SUM([Quantity]*[UnitCost]) FROM Store.OrderDetails
                    WHERE OrderId = @OrderId;
                RETURN coalesce(@Result, 0)
                END;";
            migrationBuilder.Sql(sql);
        }

        public static void DropOrderTotalFunction(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("drop function [Store].[GetOrderTotal]");
        }
    }

}