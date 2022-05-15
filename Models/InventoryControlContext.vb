Imports System.Data.Entity

Namespace Models
    Public Class InventoryControlContext
        Inherits DbContext

        Public Property Items As DbSet(Of Item)
        Public Property Categories As DbSet(Of Category)
        Public Property Orders As DbSet(Of Order)
        Public Property OrderDetails As DbSet(Of OrderDetail)
        Public Property ReturnItems As DbSet(Of ReturnItem)
        Public Property ReturnDetails As DbSet(Of ReturnDetail)
        Public Property Histories As DbSet(Of History)
        Public Property Shippings As DbSet(Of Models.Shipping)
        Public Property ShippingDetails As DbSet(Of Models.ShippingDetail)
        Public Property Inventories As DbSet(Of Inventory)
    End Class
End Namespace