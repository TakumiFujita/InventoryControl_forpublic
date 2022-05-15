Imports System
Imports System.Data.Entity.Migrations
Imports Microsoft.VisualBasic

Namespace Migrations
    Public Partial Class AddShippingAndShippingDetail
        Inherits DbMigration
    
        Public Overrides Sub Up()
            CreateTable(
                "dbo.Shippings",
                Function(c) New With
                    {
                        .ShippingId = c.Int(nullable := False, identity := True),
                        .ShippedTime = c.DateTime(nullable := False),
                        .ShippingMonth = c.DateTime(nullable := False),
                        .ShippingAmount = c.Int(nullable := False)
                    }) _
                .PrimaryKey(Function(t) t.ShippingId)
            
            CreateTable(
                "dbo.ShippingDetails",
                Function(c) New With
                    {
                        .ShippingDetailId = c.Int(nullable := False, identity := True),
                        .ItemId = c.Int(nullable := False),
                        .ShippingQuantity = c.Int(nullable := False),
                        .ShippingId = c.Int(nullable := False)
                    }) _
                .PrimaryKey(Function(t) t.ShippingDetailId) _
                .ForeignKey("dbo.Items", Function(t) t.ItemId, cascadeDelete := True) _
                .ForeignKey("dbo.Shippings", Function(t) t.ShippingId, cascadeDelete := True) _
                .Index(Function(t) t.ItemId) _
                .Index(Function(t) t.ShippingId)
            
        End Sub
        
        Public Overrides Sub Down()
            DropForeignKey("dbo.ShippingDetails", "ShippingId", "dbo.Shippings")
            DropForeignKey("dbo.ShippingDetails", "ItemId", "dbo.Items")
            DropIndex("dbo.ShippingDetails", New String() { "ShippingId" })
            DropIndex("dbo.ShippingDetails", New String() { "ItemId" })
            DropTable("dbo.ShippingDetails")
            DropTable("dbo.Shippings")
        End Sub
    End Class
End Namespace
