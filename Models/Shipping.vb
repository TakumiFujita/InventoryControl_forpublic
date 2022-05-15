Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations

Namespace Models
    Public Class Shipping
        <Key()>
        <DisplayName("出荷番号")>
        <Required(ErrorMessage:="{0}は必須です。")>
        <Range(0, 99999, ErrorMessage:="{0}は5桁以内で入力してください。")>
        Public Property ShippingId As Integer

        <DisplayName("出荷日時")>
        <Required(ErrorMessage:="{0}は必須です。")>
        <DisplayFormat(DataFormatString:="{0:yyyy-MM-dd hh:mm:ss}")>
        Public Property ShippedTime As DateTime

        <DisplayName("出荷月")>
        <Required(ErrorMessage:="{0}は必須です。")>
        <DisplayFormat(DataFormatString:="{0:yyyy-MM-dd}")>
        Public Property ShippingMonth As DateTime

        <DisplayName("出荷合計金額")>
        Public Property ShippingAmount As Integer

        <DisplayName("出荷明細")>
        Public Overridable Property ShippingDetails() As IList(Of ShippingDetail)
    End Class
End Namespace
