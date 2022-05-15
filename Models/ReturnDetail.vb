Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Models
    Public Class ReturnDetail
        <Key()>
        <DisplayName("返品明細番号")>
        <Required(ErrorMessage:="{0}は必須です。")>
        <Range(0, 99999, ErrorMessage:="{0}は5桁以内で入力してください。")>
        Public Property ReturnDetailId As Integer

        <DisplayName("商品番号")>
        <Required(ErrorMessage:="{0}は必須です。")>
        <Range(10000000, 99999999, ErrorMessage:="{0}は8桁以内で入力してください。")>
        Public Property ItemId As Integer

        <DisplayName("返品数")>
        <Required(ErrorMessage:="{0}は必須です。")>
        <Range(1, 999, ErrorMessage:="{0}は1以上の数値で入力してください。")>
        <RegularExpression("^[0-9]{1,3}", ErrorMessage:="{0}は3桁以内の正の整数で入力してください。")>
        Public Property ReturnQuantity As Integer

        <DisplayName("返品番号")>
        <Required(ErrorMessage:="{0}は必須です。")>
        <Range(0, 99999, ErrorMessage:="{0}は5桁以内で入力してください。")>
        Public Property ReturnId As Integer

        <DisplayName("削除フラグ")>
        <NotMapped()>
        Public Property IsDelete As Boolean

        <DisplayName("返品")>
        Public Overridable Property ReturnItem As ReturnItem

        <DisplayName("商品")>
        Public Overridable Property Item As Item
    End Class
End Namespace
