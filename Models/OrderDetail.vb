Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Models
    Public Class OrderDetail
        <Key()>
        <DisplayName("発注明細番号")>
        <Required(ErrorMessage:="{0}は必須です。")>
        <Range(0, 99999, ErrorMessage:="{0}は5桁以内で入力してください。")>
        Public Property OrderDetailId As Integer

        <DisplayName("商品番号")>
        <Required(ErrorMessage:="{0}は必須です。")>
        <Range(10000000, 99999999, ErrorMessage:="{0}は8桁で入力してください。")>
        Public Property ItemId As Integer

        <DisplayName("発注数")>
        <Required(ErrorMessage:="{0}は必須です。")>
        <Range(1, 999, ErrorMessage:="{0}は1以上の数値で入力してください。")>
        <RegularExpression("^[0-9]{1,3}", ErrorMessage:="{0}は3桁以内の正の整数で入力してください。")>
        Public Property OrderQuantity As Integer

        <DisplayName("発注番号")>
        <Required(ErrorMessage:="{0}は必須です。")>
        <Range(0, 99999, ErrorMessage:="{0}は5桁以内で入力してください。")>
        Public Property OrderId As Integer

        <DisplayName("削除フラグ")>
        <NotMapped()>
        Public Property IsDelete As Boolean

        <DisplayName("商品")>
        Public Overridable Property Item As Item

        '<DisplayName("商品")>
        'Public Overridable Property Item As ICollection(Of Item)

        <DisplayName("発注")>
        Public Overridable Property Order As Order
    End Class
End Namespace
