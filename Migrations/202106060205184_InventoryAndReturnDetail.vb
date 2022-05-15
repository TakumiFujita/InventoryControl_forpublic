Imports System
Imports System.Data.Entity.Migrations
Imports Microsoft.VisualBasic

Namespace Migrations
    Public Partial Class InventoryAndReturnDetail
        Inherits DbMigration
    
        Public Overrides Sub Up()
            DropForeignKey("dbo.ReturnDetailItems", "ReturnDetail_ReturnDetailId", "dbo.ReturnDetails")
            DropForeignKey("dbo.ReturnDetailItems", "Item_ItemId", "dbo.Items")
            DropIndex("dbo.ReturnDetailItems", New String() { "ReturnDetail_ReturnDetailId" })
            DropIndex("dbo.ReturnDetailItems", New String() { "Item_ItemId" })
            CreateTable(
                "dbo.Inventories",
                Function(c) New With
                    {
                        .InventoryDataId = c.Int(nullable := False, identity := True),
                        .YearMonth = c.DateTime(nullable := False),
                        .ItemId = c.Int(nullable := False),
                        .OrderQuantity = c.Int(nullable := False),
                        .ShippingQuantity = c.Int(nullable := False),
                        .ReturnQuantity = c.String(nullable := False),
                        .StockQuantity = c.Int(nullable := False)
                    }) _
                .PrimaryKey(Function(t) t.InventoryDataId) _
                .ForeignKey("dbo.Items", Function(t) t.ItemId, cascadeDelete := True) _
                .Index(Function(t) t.ItemId)
            
            AlterColumn("dbo.ReturnDetails", "ReturnQuantity", Function(c) c.String(nullable := False))
            CreateIndex("dbo.ReturnDetails", "ItemId")
            AddForeignKey("dbo.ReturnDetails", "ItemId", "dbo.Items", "ItemId", cascadeDelete := True)
            DropTable("dbo.ReturnDetailItems")
        End Sub
        
        Public Overrides Sub Down()
            CreateTable(
                "dbo.ReturnDetailItems",
                Function(c) New With
                    {
                        .ReturnDetail_ReturnDetailId = c.Int(nullable := False),
                        .Item_ItemId = c.Int(nullable := False)
                    }) _
                .PrimaryKey(Function(t) New With { t.ReturnDetail_ReturnDetailId, t.Item_ItemId })
            
            DropForeignKey("dbo.Inventories", "ItemId", "dbo.Items")
            DropForeignKey("dbo.ReturnDetails", "ItemId", "dbo.Items")
            DropIndex("dbo.Inventories", New String() { "ItemId" })
            DropIndex("dbo.ReturnDetails", New String() { "ItemId" })
            AlterColumn("dbo.ReturnDetails", "ReturnQuantity", Function(c) c.String(nullable := False, maxLength := 3))
            DropTable("dbo.Inventories")
            CreateIndex("dbo.ReturnDetailItems", "Item_ItemId")
            CreateIndex("dbo.ReturnDetailItems", "ReturnDetail_ReturnDetailId")
            AddForeignKey("dbo.ReturnDetailItems", "Item_ItemId", "dbo.Items", "ItemId", cascadeDelete := True)
            AddForeignKey("dbo.ReturnDetailItems", "ReturnDetail_ReturnDetailId", "dbo.ReturnDetails", "ReturnDetailId", cascadeDelete := True)
        End Sub
    End Class
End Namespace
