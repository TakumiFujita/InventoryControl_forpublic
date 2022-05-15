Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.Entity
Imports System.Linq
Imports System.Net
Imports System.Web
Imports System.Web.Mvc
Imports InventoryControl.Models

Namespace Controllers
    Public Class InventoriesController
        Inherits System.Web.Mvc.Controller

        Private db As New InventoryControlContext

        Public Sub New()
            Dim now = DateTime.Now
            Dim yearMonth = New DateTime(now.Year, now.Month, 1).AddMonths(1)
        End Sub

        ' GET: Inventories
        Function Index() As ActionResult
            Dim inventories = db.Inventories.Include(Function(i) i.Item)
            Return View(inventories.ToList())
        End Function

        ' GET: Inventories/Details/5
        Function Details(ByVal id As Integer?) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim inventory As Inventory = db.Inventories.Find(id)
            If IsNothing(inventory) Then
                Return HttpNotFound()
            End If
            Return View(inventory)
        End Function

        ' GET: Inventories/Create
        Function Create() As ActionResult
            ViewBag.ItemId = New SelectList(db.Items, "ItemId", "ItemName")
            Return View()
        End Function

        ' POST: Inventories/Create
        '過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        '詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 をご覧ください。
        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function Create(<Bind(Include:="InventoryDataId,YearMonth,ItemId,LastMonthsStockQuantity,OrderQuantity,ShippingQuantity,ReturnQuantity,StockQuantity")> ByVal inventory As Inventory) As ActionResult
            If ModelState.IsValid Then
                db.Inventories.Add(inventory)
                db.SaveChanges()
                Return RedirectToAction("Index")
            End If
            ViewBag.ItemId = New SelectList(db.Items, "ItemId", "ItemName", inventory.ItemId)
            Return View(inventory)
        End Function

        ' GET: Inventories/Edit/5
        Function Edit(ByVal id As Integer?) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim inventory As Inventory = db.Inventories.Find(id)
            If IsNothing(inventory) Then
                Return HttpNotFound()
            End If
            ViewBag.ItemId = New SelectList(db.Items, "ItemId", "ItemName", inventory.ItemId)
            Return View(inventory)
        End Function

        ' POST: Inventories/Edit/5
        '過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        '詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 をご覧ください。
        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function Edit(<Bind(Include:="InventoryDataId,YearMonth,ItemId,LastMonthsStockQuantity,OrderQuantity,ShippingQuantity,ReturnQuantity,StockQuantity")> ByVal inventory As Inventory) As ActionResult
            If ModelState.IsValid Then
                db.Entry(inventory).State = EntityState.Modified
                db.SaveChanges()
                Return RedirectToAction("Index")
            End If
            ViewBag.ItemId = New SelectList(db.Items, "ItemId", "ItemName", inventory.ItemId)
            Return View(inventory)
        End Function

        ' GET: Inventories/Delete/5
        Function Delete(ByVal id As Integer?) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim inventory As Inventory = db.Inventories.Find(id)
            If IsNothing(inventory) Then
                Return HttpNotFound()
            End If
            Return View(inventory)
        End Function

        ' POST: Inventories/Delete/5
        <HttpPost()>
        <ActionName("Delete")>
        <ValidateAntiForgeryToken()>
        Function DeleteConfirmed(ByVal id As Integer) As ActionResult
            Dim inventory As Inventory = db.Inventories.Find(id)
            db.Inventories.Remove(inventory)
            db.SaveChanges()
            Return RedirectToAction("Index")
        End Function

        'Inventoriesテーブルの更新
        '在庫一覧画面で月選択ドロップダウンリストを変更する際、最新状態を取得できるようにするため、発注・返品・出荷の登録、削除の度にこのメソッドを実行する
        Sub Update(yearmonth As DateTime)
            '翌月の1日の日付を取得
            Dim lastyearmonth = New DateTime(Now.Year, Now.Month, 1)

            '①Inventoryテーブルの（対象月の）データを削除
            Dim deleteInventories = From inventory In db.Inventories
                                    Where inventory.YearMonth = yearmonth
                                    Select inventory

            For Each inventory In deleteInventories
                db.Inventories.Remove(inventory)
            Next

            db.SaveChanges()


            '②Select Insert LINQで商品テーブルから商品を追加する（入庫数や出荷数などすべて0で登録）※日付以外
            db.Database.ExecuteSqlCommand("INSERT INTO Inventories
                                           SELECT {0} AS YearMonth, ItemId, 0 AS OrderQuantity, 0 AS ShippingQuantity, 0 AS ReturnQuantity, 0 AS StockQuantity, 0 AS LastMonthsStockQuantity
                                           FROM  Items", yearmonth)

            '③前月繰越、発注数、出荷数…と値を更新していく　Update Join エンティティフレームワーク

            '前月繰越の更新
            db.Database.ExecuteSqlCommand("UPDATE Inventories
                                           SET LastMonthsStockQuantity = T.StockQuantity
                                           FROM Inventories AS i
                                           INNER JOIN (
                                               SELECT ItemId, StockQuantity
                                               FROM Inventories
                                               WHERE YearMonth = {0}
                                           ) AS T
                                           ON i.ItemId = T.ItemId
                                           WHERE i.YearMonth = {1}
                                           ", lastyearmonth, yearmonth)


            '発注数の更新
            db.Database.ExecuteSqlCommand("UPDATE Inventories
                                           SET OrderQuantity = T.Quantity
                                           FROM Inventories AS i
                                           INNER JOIN (
                                               SELECT o.OrderMonth, od.ItemId, SUM(od.OrderQuantity) AS Quantity
                                               FROM Orders AS o
                                               INNER JOIN OrderDetails  AS od
                                               ON o.OrderId = od.OrderId
                                               WHERE OrderMonth = {0}
                                               GROUP BY o.OrderMonth, od.ItemId
                                           ) AS T
                                           ON i.YearMonth = T.OrderMonth AND i.ItemId = T.ItemId
                                           ", yearmonth)


            '返品数の更新
            db.Database.ExecuteSqlCommand("UPDATE Inventories
                                           SET ReturnQuantity = T.Quantity
                                           FROM Inventories AS i
                                           INNER JOIN (
	                                           SELECT r.ReturnMonth, rd.ItemId, SUM(rd.ReturnQuantity) AS Quantity
	                                           FROM ReturnItems AS r
	                                           INNER JOIN ReturnDetails AS rd
	                                           ON r.ReturnId = rd.ReturnId
	                                           WHERE ReturnMonth = {0}
	                                           GROUP BY r.ReturnMonth, rd.ItemId
                                           ) AS T
                                           ON i.YearMonth = T.ReturnMonth AND i.ItemId =  T.ItemId
                                           ", yearmonth)


            '出荷数の更新
            db.Database.ExecuteSqlCommand("UPDATE Inventories
                                           SET ShippingQuantity = T.Quantity
                                           FROM Inventories AS i
                                           INNER JOIN (
	                                           SELECT s.ShippingMonth, sd.ItemId, SUM(sd.ShippingQuantity) AS Quantity
	                                           FROM Shippings AS s
	                                           INNER JOIN ShippingDetails AS sd
	                                           ON s.ShippingId = sd.ShippingId
	                                           WHERE ShippingMonth = {0}
	                                           GROUP BY s.ShippingMonth, sd.ItemId
                                           ) AS T
                                           ON i.YearMonth = T.ShippingMonth AND i.ItemId =  T.ItemId
                                           ", yearmonth)


            '在庫数の更新
            db.Database.ExecuteSqlCommand("UPDATE Inventories
                                           SET STockQuantity = LastMonthsStockQuantity + OrderQuantity - ReturnQUantity - ShippingQuantity
                                           WHERE YearMonth = {0}
                                           ", yearmonth)
        End Sub

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing) Then
                db.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub
    End Class
End Namespace
