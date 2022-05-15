Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations

Namespace Models
    Public Class Order
        <Key()>
        <DisplayName("発注番号")>
        <Required(ErrorMessage:="{0}は必須です。")>
        <Range(0, 99999, ErrorMessage:="{0}は5桁以内で入力してください。")>
        Public Property OrderId As Integer

        <DisplayName("発注日時")>
        <Required(ErrorMessage:="{0}は必須です。")>
        <DisplayFormat(DataFormatString:="{0:yyyy-MM-dd hh:mm:ss}")>
        Public Property OrderedTime As DateTime

        <DisplayName("発注月")>
        <Required(ErrorMessage:="{0}は必須です。")>
        <DisplayFormat(DataFormatString:="{0:yyyy-MM-dd}")>
        Public Property OrderMonth As DateTime

        <DisplayName("発注合計金額")>
        Public Property OrderAmount As Integer

        <DisplayName("発注明細")>
        Public Overridable Property OrderDetails() As IList(Of OrderDetail)
    End Class
End Namespace
