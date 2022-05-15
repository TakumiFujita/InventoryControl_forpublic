Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations

Namespace Models
    Public Class Inventory
        <Key()>
        <DisplayName("在庫データ番号")>
        <Required(ErrorMessage:="{0}は必須です。")>
        <Range(0, 99999, ErrorMessage:="{0}は5桁以内で入力してください。")>
        Public Property InventoryDataId As Integer

        <DisplayName("年月")>
        <Required(ErrorMessage:="{0}は必須です。")>
        <DisplayFormat(DataFormatString:="{0:yyyy-MM}")>
        Public Property YearMonth As DateTime

        <DisplayName("商品番号")>
        <Required(ErrorMessage:="{0}は必須です。")>
        <Range(10000000, 99999999, ErrorMessage:="{0}は8桁で入力してください。")>
        Public Property ItemId As Integer

        <DisplayName("前月の在庫数")>
        <Required(ErrorMessage:="{0}は必須です。")>
        <Range(1, 9999, ErrorMessage:="{0}は1以上の数値で入力してください。")>
        <RegularExpression("^[0-9]{1,4}", ErrorMessage:="{0}は4桁以内の正の整数で入力してください。")>
        Public Property LastMonthsStockQuantity As Integer

        <DisplayName("発注数")>
        <Required(ErrorMessage:="{0}は必須です。")>
        <Range(1, 999, ErrorMessage:="{0}は1以上の数値で入力してください。")>
        <RegularExpression("^[0-9]{1,3}", ErrorMessage:="{0}は3桁以内の正の整数で入力してください。")>
        Public Property OrderQuantity As Integer

        <DisplayName("出荷数")>
        <Required(ErrorMessage:="{0}は必須です。")>
        <Range(1, 999, ErrorMessage:="{0}は1以上の数値で入力してください。")>
        <RegularExpression("^[0-9]{1,3}", ErrorMessage:="{0}は3桁以内の正の整数で入力してください。")>
        Public Property ShippingQuantity As Integer

        <DisplayName("返品数")>
        <Required(ErrorMessage:="{0}は必須です。")>
        <Range(1, 999, ErrorMessage:="{0}は1以上の数値で入力してください。")>
        <RegularExpression("^[0-9]{1,3}", ErrorMessage:="{0}は3桁以内の正の整数で入力してください。")>
        Public Property ReturnQuantity As String

        <DisplayName("在庫数")>
        <Required(ErrorMessage:="{0}は必須です。")>
        <Range(1, 9999, ErrorMessage:="{0}は1以上の数値で入力してください。")>
        <RegularExpression("^[0-9]{1,4}", ErrorMessage:="{0}は4桁以内の正の整数で入力してください。")>
        Public Property StockQuantity As Integer

        <DisplayName("商品")>
        Public Overridable Property Item As Item
    End Class
End Namespace
