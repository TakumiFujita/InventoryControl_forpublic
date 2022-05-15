Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Models
    Public Class Item
        <Key()>
        <DisplayName("商品番号")>
        <Required(ErrorMessage:="{0}は必須です。")>
        <DatabaseGenerated(DatabaseGeneratedOption.None)>
        <Range(10000000, 99999999, ErrorMessage:="{0}は8桁で入力してください。")>
        Public Property ItemId As Integer

        <DisplayName("商品名")>
        <Required(ErrorMessage:="{0}は必須です。")>
        <Index(IsUnique:=True)>
        <StringLength(16, ErrorMessage:="{0}は{1}文字以内で入力してください。")>
        Public Property ItemName As String

        <DisplayName("カテゴリー番号")>
        <Required(ErrorMessage:="カテゴリー名は必須です。")>
        <StringLength(3, ErrorMessage:="{0}は{1}桁以内で入力してください。")>
        Public Property CategoryId As String

        <DisplayName("商品単価")>
        <Required(ErrorMessage:="{0}は必須です。")>
        <Range(0, 99999, ErrorMessage:="{0}は5桁以内で入力してください。")>
        <DisplayFormat(DataFormatString:="{0:N0}", ApplyFormatInEditMode:=True)>
        Public Property UnitPrice As Integer

        <DisplayName("カテゴリー")>
        Public Overridable Property Category As Category

        <DisplayName("発注明細")>
        Public Overridable Property OrderDetails() As ICollection(Of OrderDetail)

        <DisplayName("返品明細")>
        Public Overridable Property ReturnDetails() As ICollection(Of ReturnDetail)

        <DisplayName("出荷明細")>
        Public Overridable Property ShippingDetails() As ICollection(Of ShippingDetail)
    End Class

    Public Class ItemViewModel

        Public Property ItemId As Integer

        Public Property ItemName As String

        Public Property CategoryId As String

        Public Property CategoryName As String

        <DisplayFormat(DataFormatString:="{0:N0}", ApplyFormatInEditMode:=True)>
        Public Property UnitPrice As Integer

        Public Property OrderQuantity As Integer

        'Trueであれば、商品検索画面で「追加する」ボタンを表示する
        Public Property IsSelectable As Boolean = True
    End Class
End Namespace