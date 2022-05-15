Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations

Namespace Models
    Public Class ReturnItem
        <Key()>
        <DisplayName("返品番号")>
        <Required(ErrorMessage:="{0}は必須です。")>
        <Range(0, 99999, ErrorMessage:="{0}は5桁以内で入力してください。")>
        Public Property ReturnId As Integer

        <DisplayName("返品日時")>
        <Required(ErrorMessage:="{0}は必須です。")>
        <DisplayFormat(DataFormatString:="{0:yyyy-MM-dd hh:mm:ss}")>
        Public Property ReturnedTime As DateTime

        <DisplayName("返品月")>
        <Required(ErrorMessage:="{0}は必須です。")>
        <DisplayFormat(DataFormatString:="{0:yyyy-MM-dd}")>
        Public Property ReturnMonth As DateTime

        <DisplayName("返品合計金額")>
        Public Property ReturnAmount As Integer

        <DisplayName("返品明細")>
        Public Overridable Property ReturnDetails() As IList(Of ReturnDetail)
    End Class
End Namespace
