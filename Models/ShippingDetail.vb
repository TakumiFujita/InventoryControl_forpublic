Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Models
    Public Class ShippingDetail
        <Key()>
        <DisplayName("出荷明細番号")>
        <Required(ErrorMessage:="{0}は必須です。")>
        <Range(0, 99999, ErrorMessage:="{0}は5桁以内で入力してください。")>
        Public Property ShippingDetailId As Integer

        <DisplayName("商品番号")>
        <Required(ErrorMessage:="{0}は必須です。")>
        <Range(10000000, 99999999, ErrorMessage:="{0}は8桁で入力してください。")>
        Public Property ItemId As Integer

        <DisplayName("出荷数")>
        <Required(ErrorMessage:="{0}は必須です。")>
        <Range(1, 999, ErrorMessage:="{0}は1以上の数値で入力してください。")>
        <RegularExpression("^[0-9]{1,3}", ErrorMessage:="{0}は3桁以内の正の整数で入力してください。")>
        Public Property ShippingQuantity As Integer

        <DisplayName("出荷番号")>
        <Required(ErrorMessage:="{0}は必須です。")>
        <Range(0, 99999, ErrorMessage:="{0}は5桁以内で入力してください。")>
        Public Property ShippingId As Integer

        <DisplayName("削除フラグ")>
        <NotMapped()>
        Public Property IsDelete As Boolean

        <DisplayName("商品")>
        Public Overridable Property Item As Item

        <DisplayName("出荷")>
        Public Overridable Property Shipping As Shipping
    End Class
End Namespace
