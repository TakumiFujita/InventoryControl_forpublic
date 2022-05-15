Imports System
Imports System.Data.Entity.Migrations
Imports Microsoft.VisualBasic

Namespace Migrations
    Public Partial Class ModifyReturnDetail
        Inherits DbMigration
    
        Public Overrides Sub Up()
            AlterColumn("dbo.ReturnDetails", "ReturnQuantity", Function(c) c.Int(nullable := False))
        End Sub
        
        Public Overrides Sub Down()
            AlterColumn("dbo.ReturnDetails", "ReturnQuantity", Function(c) c.String(nullable := False))
        End Sub
    End Class
End Namespace
