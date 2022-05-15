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
    Public Class OrdersController
        Inherits System.Web.Mvc.Controller

        Private db As New InventoryControlContext

        Public Sub New()
            Dim now = DateTime.Now
            Dim orderMonth = New DateTime(now.Year, now.Month, 1).AddMonths(1)

            ViewBag.orderMonth = orderMonth
            ViewBag.displayOrderMonth = orderMonth.ToString("yyyy年MM月")
        End Sub

        ' GET: Orders
        Function Index() As ActionResult
            Return View(db.Orders.ToList())
        End Function

        ' GET: Orders/Details/5
        Function Details(ByVal id As Integer?) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim order As Order = db.Orders.Find(id)
            If IsNothing(order) Then
                Return HttpNotFound()
            End If
            Return View(order)
        End Function

        Function SearchItem(<Bind(Include:="ItemId,CategoryName,ItemName")> ByVal model As SearchViewModel) As ActionResult
            If Request.IsAjaxRequest Then
                Dim searchmodel = From item In db.Items
                                  Select New ItemViewModel With {
                                    .ItemId = item.ItemId,
                                    .ItemName = item.ItemName,
                                    .CategoryId = item.CategoryId,
                                    .UnitPrice = item.UnitPrice
                                    }

                model.Items = searchmodel.ToList()

                Return PartialView("_SearchItem", model)
            End If
            Return View()
        End Function

        Function AddToCart(<Bind(Include:="OrderQuantity")> ByVal model As OrderDetail) As ActionResult
            'カートに入っている各商品の発注数を登録する
            Dim ordequantity = model.OrderQuantity


            '検索画面へ遷移
            Return RedirectToAction("Search")
        End Function

        Function Search() As ActionResult
            ViewBag.orderMonth = DateTime.Now.AddMonths(1).ToString("yyyy年MM月")

            Dim categories = From c In db.Categories
                             Select New With {.CategoryId = c.CategoryId, .CategoryName = c.CategoryName}

            '第1引数:ドロップダウンリストの選択肢の中身（リスト）,第2引数:Value属性に割り当てされるもの（リストの主キーの列名）,
            '第3引数:Text属性に割り当てされるもの（リストの表示名の列名）,第4引数:初期値（予め選択されている項目）
            ViewBag.SelectCategory = New SelectList(categories, "CategoryId", "CategoryName", Nothing)


            ViewBag.SelectCategory2 = db.Categories.Select(Function(category) New SelectListItem With {
                                                             .Text = category.CategoryName,
                                                             .Value = category.CategoryId})

            '既にカート（発注明細テーブル）へ追加済みの商品の行数（数）を取得し、ViewBagへ格納
            ViewBag.orderdetailsRowCount = db.Orders.Where(Function(order) order.OrderAmount = 0).Select(Function(order) order.OrderDetails.Count()).FirstOrDefault()

            '既にカート（発注明細テーブル）へ追加済みの商品の行数（数）を取得し、ViewBagへ格納（上記コードの書き換え）
            'Dim orderid = db.Orders.Where(Function(order) order.OrderAmount = 0).Select(Function(order) order.OrderId).FirstOrDefault()
            'ViewBag.orderdetailsRowCount = db.OrderDetails.Where(Function(orderdetail) orderdetail.OrderId = orderid).Select(Function(orderdetail) orderdetail.ItemId).Count()

            Dim viewModel = New SearchViewModel()

            'Itemsプロパティがクラスなので、まだNew（インスタンス化）されていないため、以下でNewを行う
            'viewModel.Items = New List(Of Item)

            Return View(viewModel)
        End Function

        <HttpPost()>
        Function Search(<Bind(Include:="ItemId,CategoryId,ItemName")> ByVal model As SearchViewModel) As ActionResult
            'If Request.IsAjaxRequest Then
            '条件が指定されなかった場合、全件検索するためのクエリ
            Dim searchmodel = From item In db.Items
                              Select New ItemViewModel With {
                                    .ItemId = item.ItemId,
                                    .ItemName = item.ItemName,
                                    .CategoryId = item.CategoryId,
                                    .CategoryName = item.Category.CategoryName,
                                    .UnitPrice = item.UnitPrice
                                    }

            'ユーザー入力に対し、検索で引っかかったものを結果として返す

            'メソッド式
            'searchmodel = searchmodel.Where(Function(item) (Not model.ItemId Is Nothing OrElse item.ItemId.ToString.Contains(model.ItemId)) And
            '                                         (String.IsNullOrEmpty(model.ItemName) OrElse item.ItemName.Contains(model.ItemName)) And
            '                                         (String.IsNullOrEmpty(model.CategoryId) OrElse item.CategoryId.Equals(model.CategoryId)
            '                                         ))

            If Not model.ItemId Is Nothing Then
                searchmodel = searchmodel.Where(Function(item) item.ItemId.ToString.Contains(model.ItemId))
            End If

            If Not String.IsNullOrEmpty(model.ItemName) Then
                searchmodel = searchmodel.Where(Function(item) item.ItemName.Contains(model.ItemName))
            End If

            If Not String.IsNullOrEmpty(model.CategoryId) Then
                searchmodel = searchmodel.Where(Function(item) item.CategoryId.Equals(model.CategoryId))
            End If

            'このタイミングでSQLが実行され、model.Itemsがセットされる　※直前までの検索はSQLを組み立てているだけ
            model.Items = searchmodel.ToList()

            'もし商品のカートへの追加が二回目以降の場合、発注番号が既に登録されているはずなので、その発注番号を取得する
            Dim orderid = db.Orders.Where(Function(order) order.OrderAmount = 0).Select(Function(order) order.OrderId).FirstOrDefault()

            '既にカート（発注明細テーブル）へ追加済みの商品番号の取得
            Dim addedItemIdArray = db.OrderDetails.Where(Function(orderdetail) orderdetail.OrderId = orderid).Select(Function(orderdetail) orderdetail.ItemId)

            '上記で取得した（既に追加済みの）商品番号を一つずつ取り出す
            For Each addedItemId In addedItemIdArray
                '検索結果として得られた商品に対して、その商品番号が、追加済みの商品番号と一致するかを確認
                For i = 0 To model.Items.Count - 1
                    '同じ商品番号が配列の中に存在していた場合
                    If model.Items(i).ItemId = addedItemId Then
                        'IsSelectableプロパティをFalseにする（「追加」ボタンを非表示にする）
                        model.Items(i).IsSelectable = False
                    End If
                Next
            Next

            ViewBag.SelectCategory2 = db.Categories.Select(Function(category) New SelectListItem With {
                                                         .Text = category.CategoryName,
                                                         .Value = category.CategoryId})

            '検索ボタンをクリックすると、「ViewBag.orderdetailsRowCount」が消えてしまうため、再度記述
            ViewBag.orderdetailsRowCount = db.Orders.Where(Function(order) order.OrderAmount = 0).Select(Function(order) order.OrderDetails.Count()).FirstOrDefault()

            Return View(model)
        End Function

        '<HttpPost()>
        Function AddItem(ByVal id As Integer) As ActionResult
            'もし商品のカートへの追加が二回目以降の場合、発注番号が既に登録されているはずなので、その発注番号を取得する
            Dim orderid = db.Orders.Where(Function(order) order.OrderAmount = 0).Select(Function(order) order.OrderId).FirstOrDefault()

            '上記で発注番号が作成されていない場合は、新しくOrderレコードを追加する
            If orderid = 0 Then
                '発注番号が発注明細テーブル登録時に必要なため、発注テーブルへ仮登録
                Dim result = db.Orders.Add(New Order With {
                                   .OrderedTime = DateTime.Now,
                                   .OrderMonth = DirectCast(ViewBag.orderMonth, DateTime),
                                   .OrderAmount = 0
                                   })
                db.SaveChanges()
                orderid = result.OrderId
            End If

            '発注明細テーブルへitemIdを登録
            db.OrderDetails.Add(New OrderDetail With {
                               .ItemId = id,
                               .OrderQuantity = 1,
                               .OrderId = orderid
                               })

            db.SaveChanges()

            Return RedirectToAction("Create")
        End Function

        '<HttpPost()>
        'Function AddItem(ByVal id As Integer, <Bind(Include:="ItemId,CategoryId,ItemName")> ByVal item As Item) As ActionResult
        '    Return View()
        'End Function

        '選択したカテゴリーを、検索後に再表示するためのSub関数
        'Private Sub SetCategoryDropdownItems()
        '    Dim list = db.Categories.Select(Function(category) New SelectListItem With {
        '                                     .Text = category.CategoryName,
        '                                     .Value = category.CategoryId})

        '    list.Insert(0, New SelectListItem With {
        '        .Text = String.Empty,
        '        .Value = "0"
        '    })
        'End Sub

        ' GET: Orders/Create
        Function Create() As ActionResult
            'ViewBag.orderMonth = DateTime.Now.AddMonths(1).ToString("yyyy年MM月")

            Dim categories = From c In db.Categories
                             Select New With {.CategoryId = c.CategoryId, .CategoryName = c.CategoryName}

            '第1引数:ドロップダウンリストの選択肢の中身（リスト）,第2引数:Value属性に割り当てされるもの（リストの主キーの列名）,
            '第3引数:Text属性に割り当てされるもの（リストの表示名の列名）,第4引数:初期値（予め選択されている項目）
            ViewBag.SelectCategory = New SelectList(categories, "CategoryId", "CategoryName", Nothing)


            Dim viewModel = New Order()

            'AjaxActionLinkでビューを表示する件がうまくいくまで、いったんコメントアウト
            viewModel = db.Orders.Where(Function(order) order.OrderAmount = 0).FirstOrDefault()

            'Itemsプロパティがクラスなので、まだNew（インスタンス化）されていないため、以下でNewを行う
            'viewModel.OrderDetails = (From item In db.OrderDetails
            '                         Select New OrderDetail With {
            '                        .ItemId = item.ItemId
            '                        }).ToList()

            Return View(viewModel)
        End Function

        ' POST: Orders/Create
        '過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        '詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 をご覧ください。
        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function Create(<Bind(Include:="OrderId,OrderDetails,ItemId,ItemName,UnitPrice,OrderQuantity,CategoryName,OrderedTime,OrderMonth,OrderAmount")> ByVal order As Order, submit As String) As ActionResult

            'OrderDetailsテーブルに商品が存在しない場合、そのまま画面に返す
            If order.OrderDetails Is Nothing Then
                If submit = "商品検索" Then

                    Return RedirectToAction("Search")

                ElseIf submit = "発注確定" Then

                    Return RedirectToAction("Index", "Items")

                End If
                Return View(order)
            End If

            '発注金額を計算する
            Dim orderamount As Integer

            'For Eachによる合計の算出
            For Each orderdetail In order.OrderDetails
                Dim unitprice = orderdetail.Item.UnitPrice
                Dim orderquantity = orderdetail.OrderQuantity
                orderamount += unitprice * orderquantity
            Next

            'LINQのクエリ式による合計の算出
            'orderamount = (From orderdetail In order.OrderDetails
            '               Select orderdetail.Item.UnitPrice * orderdetail.OrderQuantity).Sum()

            'LINQのメソッド式による合計の算出
            'orderamount = order.OrderDetails.Select(Function(orderdetail) orderdetail.Item.UnitPrice * orderdetail.OrderQuantity).Sum()

            '発注商品一覧画面で商品がある場合（削除して登録するものがない場合を除く）、OrderDetailsテーブルに仮登録する
            For Each orderdetail In order.OrderDetails
                '同じ商品を入れた際に保存エラーになるのを対策
                'db.Items.Attach(orderdetail.Item)　※この記述ではうまくいかなかった
                orderdetail.Item = Nothing '※OrderDetailとItemとの関連を削除すると、正常に動作した

                db.Entry(orderdetail).State = EntityState.Modified '※変更レコードとしてコンテキストに追加
            Next

            Dim now = DateTime.Now
            Dim yearmonth = New DateTime(now.Year, now.Month, 1).AddMonths(1)
            Dim InventoriesController As New InventoriesController

            If submit = "商品検索" Then

                'db.Orders.Add(order)　※Ordersテーブルへの保存は今回行わない
                'db.OrderDetails.AddRange(order.OrderDetails)　※Addは一行追加、AddRangeで複数行追加可能だが、行ごとに詳細設定が必要なためうまくいかなかった

                '変更をDBに保存する（OrderDetailsテーブル）
                db.SaveChanges()

                Return RedirectToAction("Search")

            ElseIf submit = "発注確定" Then

                '未発注（発注テーブルの中で、発注金額が0で登録されている）の行のOrderIdを取得
                Dim orderid = db.Orders.Where(Function(o) o.OrderAmount = 0).Select(Function(o) o.OrderId).FirstOrDefault()

                'Ordersテーブルに合計金額を登録する
                Dim orderModel = db.Orders.Single(Function(model) model.OrderId = orderid)
                orderModel.OrderAmount = orderamount

                '変更をDBに保存する（Ordersテーブル、OrderDetailsテーブル）
                db.SaveChanges()

                '在庫テーブルと発注テーブルの整合性を保つために在庫テーブルを更新する
                InventoriesController.Update(yearmonth)

                Return RedirectToAction("Index", "Items")

            ElseIf submit = "削除（非表示）" Then

                '発注商品一覧で削除フラグ（IsDelete）がTrueのものがあれば、行削除を行う
                For Each orderdetail In order.OrderDetails

                    '商品番号を取得
                    Dim id = orderdetail.ItemId

                    If IsNothing(id) Then
                        Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
                    End If

                    '未発注（発注テーブルの中で、発注金額が0で登録されている）の行のOrderIdを取得
                    Dim orderid = db.Orders.Where(Function(o) o.OrderAmount = 0).Select(Function(o) order.OrderId).FirstOrDefault()

                    Dim deleteorderdetail As OrderDetail = (From od In db.OrderDetails
                                                            Where od.ItemId = id AndAlso od.OrderId = orderid AndAlso orderdetail.IsDelete = True
                                                            Select od).FirstOrDefault()

                    If Not deleteorderdetail Is Nothing Then
                        db.OrderDetails.Remove(deleteorderdetail)
                    End If

                Next

                db.SaveChanges()

                '在庫テーブルと発注テーブルの整合性を保つために在庫テーブルを更新する
                InventoriesController.Update(yearmonth)

                Return RedirectToAction("Create")

            End If

            Return View(order)
        End Function

        ' GET: Orders/Edit/5
        Function Edit(ByVal id As Integer?) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim order As Order = db.Orders.Find(id)
            If IsNothing(order) Then
                Return HttpNotFound()
            End If
            Return View(order)
        End Function

        ' POST: Orders/Edit/5
        '過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        '詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 をご覧ください。
        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function Edit(<Bind(Include:="OrderId,OrderedTime,OrderMonth,OrderAmount")> ByVal order As Order) As ActionResult
            If ModelState.IsValid Then
                db.Entry(order).State = EntityState.Modified
                db.SaveChanges()
                Return RedirectToAction("Index")
            End If
            Return View(order)
        End Function

        Function Cancel() As ActionResult
            '未発注（発注テーブルの中で、発注金額が0で登録されている）の行のOrderIdを取得
            Dim orderid = db.Orders.Where(Function(order) order.OrderAmount = 0).Select(Function(order) order.OrderId).FirstOrDefault()

            '上記で取得したOrderIdを持つ、発注テーブルのデータを削除　※紐づく発注明細テーブルの商品も自動的に削除される
            Dim orderData As Order = db.Orders.Find(orderid)

            If Not orderData Is Nothing Then
                db.Orders.Remove(orderData)
                db.SaveChanges()
            End If

            Return RedirectToAction("Index", "Items")
        End Function

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing) Then
                db.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub
    End Class
End Namespace
