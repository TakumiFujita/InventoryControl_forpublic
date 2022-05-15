Imports System
Imports System.Data.Entity.Migrations
Imports Microsoft.VisualBasic

Namespace Migrations
    Public Partial Class InitialCreate
        Inherits DbMigration
    
        Public Overrides Sub Up()
            CreateTable(
                "dbo.Categories",
                Function(c) New With
                    {
                        .CategoryId = c.String(nullable := False, maxLength := 3),
                        .CategoryName = c.String(nullable := False, maxLength := 8)
                    }) _
                .PrimaryKey(Function(t) t.CategoryId) _
                .Index(Function(t) t.CategoryName, unique := True)
            
            CreateTable(
                "dbo.Items",
                Function(c) New With
                    {
                        .ItemId = c.Int(nullable := False),
                        .ItemName = c.String(nullable := False, maxLength := 16),
                        .CategoryId = c.String(nullable := False, maxLength := 3),
                        .UnitPrice = c.Int(nullable := False)
                    }) _
                .PrimaryKey(Function(t) t.ItemId) _
                .ForeignKey("dbo.Categories", Function(t) t.CategoryId, cascadeDelete := True) _
                .Index(Function(t) t.ItemName, unique := True) _
                .Index(Function(t) t.CategoryId)
            
            CreateTable(
                "dbo.OrderDetails",
                Function(c) New With
                    {
                        .OrderDetailId = c.Int(nullable := False, identity := True),
                        .ItemId = c.Int(nullable := False),
                        .OrderQuantity = c.Int(nullable := False),
                        .OrderId = c.Int(nullable := False)
                    }) _
                .PrimaryKey(Function(t) t.OrderDetailId) _
                .ForeignKey("dbo.Items", Function(t) t.ItemId, cascadeDelete := True) _
                .ForeignKey("dbo.Orders", Function(t) t.OrderId, cascadeDelete := True) _
                .Index(Function(t) t.ItemId) _
                .Index(Function(t) t.OrderId)
            
            CreateTable(
                "dbo.Orders",
                Function(c) New With
                    {
                        .OrderId = c.Int(nullable := False, identity := True),
                        .OrderedTime = c.DateTime(nullable := False),
                        .OrderMonth = c.DateTime(nullable := False),
                        .OrderAmount = c.Int(nullable := False)
                    }) _
                .PrimaryKey(Function(t) t.OrderId)
            
            CreateTable(
                "dbo.ReturnDetails",
                Function(c) New With
                    {
                        .ReturnDetailId = c.Int(nullable := False, identity := True),
                        .ItemId = c.Int(nullable := False),
                        .ReturnQuantity = c.String(nullable := False, maxLength := 3),
                        .ReturnId = c.Int(nullable := False)
                    }) _
                .PrimaryKey(Function(t) t.ReturnDetailId) _
                .ForeignKey("dbo.ReturnItems", Function(t) t.ReturnId, cascadeDelete := True) _
                .Index(Function(t) t.ReturnId)
            
            CreateTable(
                "dbo.ReturnItems",
                Function(c) New With
                    {
                        .ReturnId = c.Int(nullable := False, identity := True),
                        .ReturnedTime = c.DateTime(nullable := False),
                        .ReturnMonth = c.DateTime(nullable := False),
                        .ReturnAmount = c.Int(nullable := False)
                    }) _
                .PrimaryKey(Function(t) t.ReturnId)
            
            CreateTable(
                "dbo.Histories",
                Function(c) New With
                    {
                        .TransactionId = c.Int(nullable := False, identity := True),
                        .TransactionTime = c.DateTime(nullable := False),
                        .TransactionType = c.Int(nullable := False)
                    }) _
                .PrimaryKey(Function(t) t.TransactionId)
            
            CreateTable(
                "dbo.ReturnDetailItems",
                Function(c) New With
                    {
                        .ReturnDetail_ReturnDetailId = c.Int(nullable := False),
                        .Item_ItemId = c.Int(nullable := False)
                    }) _
                .PrimaryKey(Function(t) New With { t.ReturnDetail_ReturnDetailId, t.Item_ItemId }) _
                .ForeignKey("dbo.ReturnDetails", Function(t) t.ReturnDetail_ReturnDetailId, cascadeDelete := True) _
                .ForeignKey("dbo.Items", Function(t) t.Item_ItemId, cascadeDelete := True) _
                .Index(Function(t) t.ReturnDetail_ReturnDetailId) _
                .Index(Function(t) t.Item_ItemId)
            
        End Sub
        
        Public Overrides Sub Down()
            DropForeignKey("dbo.ReturnDetails", "ReturnId", "dbo.ReturnItems")
            DropForeignKey("dbo.ReturnDetailItems", "Item_ItemId", "dbo.Items")
            DropForeignKey("dbo.ReturnDetailItems", "ReturnDetail_ReturnDetailId", "dbo.ReturnDetails")
            DropForeignKey("dbo.OrderDetails", "OrderId", "dbo.Orders")
            DropForeignKey("dbo.OrderDetails", "ItemId", "dbo.Items")
            DropForeignKey("dbo.Items", "CategoryId", "dbo.Categories")
            DropIndex("dbo.ReturnDetailItems", New String() { "Item_ItemId" })
            DropIndex("dbo.ReturnDetailItems", New String() { "ReturnDetail_ReturnDetailId" })
            DropIndex("dbo.ReturnDetails", New String() { "ReturnId" })
            DropIndex("dbo.OrderDetails", New String() { "OrderId" })
            DropIndex("dbo.OrderDetails", New String() { "ItemId" })
            DropIndex("dbo.Items", New String() { "CategoryId" })
            DropIndex("dbo.Items", New String() { "ItemName" })
            DropIndex("dbo.Categories", New String() { "CategoryName" })
            DropTable("dbo.ReturnDetailItems")
            DropTable("dbo.Histories")
            DropTable("dbo.ReturnItems")
            DropTable("dbo.ReturnDetails")
            DropTable("dbo.Orders")
            DropTable("dbo.OrderDetails")
            DropTable("dbo.Items")
            DropTable("dbo.Categories")
        End Sub
    End Class
End Namespace
