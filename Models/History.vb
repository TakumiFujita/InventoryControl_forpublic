Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations

Namespace Models
    Public Class History
        <Key()>
        <DisplayName("取引番号")>
        Public Property TransactionId As Integer

        <DisplayName("取引日時")>
        <Required(ErrorMessage:="{0}は必須です。")>
        <DisplayFormat(DataFormatString:="{0:yyyy-MM-dd hh:mm:ss}")>
        Public Property TransactionTime As DateTime

        <DisplayName("取引種別")>
        <Required(ErrorMessage:="{0}は必須です。")>
        Public Property TransactionType As Integer
    End Class
End Namespace
