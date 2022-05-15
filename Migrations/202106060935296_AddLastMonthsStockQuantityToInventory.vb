Imports System
Imports System.Data.Entity.Migrations
Imports Microsoft.VisualBasic

Namespace Migrations
    Public Partial Class AddLastMonthsStockQuantityToInventory
        Inherits DbMigration
    
        Public Overrides Sub Up()
            AddColumn("dbo.Inventories", "LastMonthsStockQuantity", Function(c) c.Int(nullable := False))
        End Sub
        
        Public Overrides Sub Down()
            DropColumn("dbo.Inventories", "LastMonthsStockQuantity")
        End Sub
    End Class
End Namespace
