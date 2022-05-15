Imports System.ComponentModel

Namespace Models
    Public Class InventoryListViewModel
        <DisplayName("商品番号")>
        Public Property ItemId As Integer

        <DisplayName("商品名")>
        Public Property ItemName As String

        <DisplayName("カテゴリー番号")>
        Public Property CategoryId As String

        <DisplayName("カテゴリー名")>
        Public Property CategoryName As String

        <DisplayName("前月在庫数")>
        Public Property LastMonthsStockQuantity As Integer

        <DisplayName("発注数")>
        Public Property OrderQuantity As Integer

        <DisplayName("出荷数")>
        Public Property ShippingQuantity As Integer

        <DisplayName("返品数")>
        Public Property ReturnQuantity As Integer

        <DisplayName("在庫数")>
        Public Property StockQuantity As Integer

        <DisplayName("月")>
        Public Property Month As Months
    End Class
End Namespace
