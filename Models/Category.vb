Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Models
    Public Class Category
        <Key()>
        <DisplayName("カテゴリー番号")>
        <Required(ErrorMessage:="{0}は必須です。")>
        <StringLength(3, ErrorMessage:="{0}は{1}桁以内で入力してください。")>
        Public Property CategoryId As String

        <DisplayName("カテゴリー名")>
        <Required(ErrorMessage:="{0}は必須です。")>
        <Index(IsUnique:=True)>
        <StringLength(8, ErrorMessage:="{0}は{1}文字以内で入力してください。")>
        Public Property CategoryName As String

        <DisplayName("商品")>
        Public Overridable Property Items() As ICollection(Of Item)
    End Class
End Namespace