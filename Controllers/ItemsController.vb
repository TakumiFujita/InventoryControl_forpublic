Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.Entity
Imports System.Data.Entity.Infrastructure
Imports System.Data.SqlClient
Imports System.Linq
Imports System.Net
Imports System.Text.RegularExpressions
Imports System.Web
Imports System.Web.Mvc
Imports InventoryControl.Models

Namespace Controllers
    Public Class ItemsController
        Inherits System.Web.Mvc.Controller

        Private db As New InventoryControlContext

        ' GET: Items
        Function Index(ByVal value As String) As ActionResult
            Dim now = DateTime.Now
            Dim month As DateTime
            Dim thisMonth = New DateTime(now.Year, now.Month, 1)

            '初期表示の場合はアクセス月（当月）を、ドロップダウンで月選択を行った場合はその選択月をmonthに代入
            If Not value Is Nothing Then
                month = DateTime.Parse(value)
            Else
                month = thisMonth
            End If

            Dim model = From item In db.Items
                        Join category In db.Categories On item.CategoryId Equals category.CategoryId
                        Join inventory In db.Inventories On item.ItemId Equals inventory.ItemId
                        Where inventory.YearMonth = month
                        Order By item.ItemId
                        Select New InventoryListViewModel With {
                            .ItemId = item.ItemId,
                            .ItemName = item.ItemName,
                            .CategoryName = category.CategoryName,
                            .LastMonthsStockQuantity = inventory.LastMonthsStockQuantity,
                            .OrderQuantity = inventory.OrderQuantity,
                            .ShippingQuantity = inventory.ShippingQuantity,
                            .ReturnQuantity = inventory.ReturnQuantity,
                            .StockQuantity = inventory.StockQuantity
                            }

            '初期表示の場合は、アクセス月をvalueに設定
            If value Is Nothing Then
                value = New DateTime(now.Year, now.Month, 1)
            End If

            '月選択ドロップダウンリストの生成
            Dim selectListItems = New List(Of SelectListItem)

            Dim d = New DateTime(now.Year, now.Month, 1).AddMonths(1)

            For i = 1 To 12
                If d = value Then
                    selectListItems.Add(New SelectListItem() With {.Value = d.ToString("yyyy/MM/01"), .Text = d.ToString("yyyy年M月"), .Selected = True})
                Else
                    selectListItems.Add(New SelectListItem() With {.Value = d.ToString("yyyy/MM/01"), .Text = d.ToString("yyyy年M月")})
                End If

                '上記の書き換え
                'Dim sli = New SelectListItem() With {.Value = d.ToString("yyyy/MM/01"), .Text = d.ToString("yyyy年M月")}
                'If i = 2 Then sli.Selected = True
                'selectListItems.Add(sli)

                '月を-1する
                d = d.AddMonths(-1)
            Next

            ViewBag.Month = selectListItems

            Return View(model)
            'Return View(db.Items.ToList())
        End Function

        ' GET: Items/ChangeDDL
        Function ChangeDDL(ByVal value As String) As ActionResult
            '選択した月（selectMonth）の在庫データを取得
            Dim inventoryData = db.Inventories.Where(Function(i) i.YearMonth = DateTime.Parse(value))
            Return View(inventoryData)
        End Function

        ' GET: Items/Details/5
        Function Details(ByVal id As Integer?) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim item As Item = db.Items.Find(id)
            If IsNothing(item) Then
                Return HttpNotFound()
            End If
            Return View(item)
        End Function

        Function SearchItem(<Bind(Include:="ItemId,CtageoryName,ItemName")> ByVal model As SearchViewModel) As ActionResult
            Dim searchmodel = From item In db.Items
                              Select New SearchViewModel With {
                            .ItemId = item.ItemId,
                            .ItemName = item.ItemName,
                            .UnitPrice = item.UnitPrice
                            }

            Return PartialView("_SearchItem", searchmodel)
        End Function

        ' GET: Items/Create
        Function Create() As ActionResult
            'カテゴリーのドロップダウンリストの表示のために、DBからデータを取得
            Dim categories = From a In db.Categories
                             Select New With {.CategoryId = a.CategoryId, .CategoryName = a.CategoryName}

            '第1引数:ドロップダウンリストの選択肢の中身（リスト）,第2引数:Value属性に割り当てされるもの（リストの主キーの列名）,
            '第3引数:Text属性に割り当てされるもの（リストの表示名の列名）,第4引数:初期値（予め選択されている項目）
            ViewBag.SelectCategory = New SelectList(categories, "CategoryId", "CategoryName", Nothing)

            Return View()
        End Function

        ' POST: Items/Create
        '過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        '詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 をご覧ください。
        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function Create(<Bind(Include:="ItemId,ItemName,CategoryId,UnitPrice")> ByVal item As Item) As ActionResult
            If ModelState.IsValid Then
                '重複確認
                'Dim newModelState = New ModelStateDictionary()

                'newModelState.AddModelError("ItemId", "test")
                'newModelState.AddModelError("ItemName", "test1")

                'ModelState.Merge(newModelState)


                db.Items.Add(item)

                Try
                    db.SaveChanges()
                Catch ex As DbUpdateException When TryCast(ex.InnerException?.InnerException, SqlException) IsNot Nothing
                    Dim sqlEx As SqlException = TryCast(ex.InnerException?.InnerException, SqlException)
                    If sqlEx.Number = 2627 Then
                        ModelState.AddModelError("EM_DuplicateItemId", "この商品番号は既に登録されています")
                    End If
                    If sqlEx.Number = 2601 Then
                        ModelState.AddModelError("EM_DuplicateItemName", "この商品名は既に登録されています")
                    End If

                    'カテゴリーのドロップダウンリストの表示のために、DBからデータを取得
                    Dim categories = From a In db.Categories
                                     Select New With {.CategoryId = a.CategoryId, .CategoryName = a.CategoryName}

                    '第1引数:ドロップダウンリストの選択肢の中身（リスト）,第2引数:Value属性に割り当てされるもの（リストの主キーの列名）,
                    '第3引数:Text属性に割り当てされるもの（リストの表示名の列名）,第4引数:初期値（予め選択されている項目）
                    ViewBag.SelectCategory = New SelectList(categories, "CategoryId", "CategoryName", Nothing)

                    Return View(item)
                End Try

                Return RedirectToAction("Index")
            End If
            Return View(item)
        End Function

        ' GET: Items/Edit/5
        Function Edit(ByVal id As Integer?) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim item As Item = db.Items.Find(id)
            If IsNothing(item) Then
                Return HttpNotFound()
            End If
            Return View(item)
        End Function

        ' POST: Items/Edit/5
        '過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        '詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 をご覧ください。
        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function Edit(<Bind(Include:="Id,ItemName,CategoryId,UnitPrice")> ByVal item As Item) As ActionResult
            If ModelState.IsValid Then
                db.Entry(item).State = EntityState.Modified
                db.SaveChanges()
                Return RedirectToAction("Index")
            End If
            Return View(item)
        End Function

        ' GET: Items/Delete/5
        Function Delete(ByVal id As Integer?) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim item As Item = db.Items.Find(id)
            If IsNothing(item) Then
                Return HttpNotFound()
            End If
            Return View(item)
        End Function

        ' POST: Items/Delete/5
        <HttpPost()>
        <ActionName("Delete")>
        <ValidateAntiForgeryToken()>
        Function DeleteConfirmed(ByVal id As Integer) As ActionResult
            Dim item As Item = db.Items.Find(id)
            db.Items.Remove(item)
            db.SaveChanges()
            Return RedirectToAction("Index")
        End Function

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing) Then
                db.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub
    End Class
End Namespace
