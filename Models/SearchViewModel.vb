Imports System.Collections.Generic
Imports System.ComponentModel


Namespace Models
    Public Class SearchViewModel

        '検索条件を入力するためのプロパティ

        <DisplayName("商品番号")>
        Public Property ItemId As Integer?

        <DisplayName("カテゴリー番号")>
        Public Property CategoryId As String

        <DisplayName("カテゴリー名")>
        Public Property CategoryName As String

        <DisplayName("商品名")>
        Public Property ItemName As String

        <DisplayName("商品単価")>
        Public Property UnitPrice As Integer


        '検索結果を表示するためのプロパティ

        'Public Property Items As List(Of Item)
        Public Property Items As New List(Of ItemViewModel)
        'Public Property Items As IEnumerable(Of Item)


        <DisplayName("発注月")>
        Public Property OrderMonth As DateTime

        <DisplayName("発注合計金額")>
        Public Property OrderAmount As Integer

    End Class
End Namespace
