Imports System.Data.Entity

'イニシャライザー
Namespace Models
    Public Class Initializer
        Inherits DropCreateDatabaseIfModelChanges(Of InventoryControlContext)

        Protected Overrides Sub Seed(context As InventoryControlContext)
            Dim items = New List(Of Item) From {
                    New Item With {
                    .ItemId = 10000001,
                    .ItemName = "iPhone12",
                    .CategoryId = "001",
                    .UnitPrice = 80000
                },
                    New Item With {
                    .ItemId = 10000002,
                    .ItemName = "iPhoneXI",
                    .CategoryId = "001",
                    .UnitPrice = 70000
                },
                    New Item With {
                    .ItemId = 20000001,
                    .ItemName = "MacBookAir",
                    .CategoryId = "002",
                    .UnitPrice = 95000
                }
            }


            Dim categories = New List(Of Category) From {
                    New Category With {
                    .CategoryId = "001",
                    .CategoryName = "スマートフォン"
                },
                    New Category With {
                    .CategoryId = "002",
                    .CategoryName = "パソコン"
                }
            }

            items.ForEach(Function(item) context.Items.Add(item))
            categories.ForEach(Function(category) context.Categories.Add(category))
            context.SaveChanges()

        End Sub

    End Class
End Namespace
