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
    Public Class ReturnsController
        Inherits System.Web.Mvc.Controller

        Private db As New InventoryControlContext

        Public Sub New()
            Dim now = DateTime.Now
            Dim returnMonth = New DateTime(now.Year, now.Month, 1).AddMonths(1)

            ViewBag.returnMonth = returnMonth
            ViewBag.displayReturnMonth = returnMonth.ToString("yyyy年MM月")
        End Sub

        ' GET: Returns
        Function Index() As ActionResult
            Return View(db.ReturnItems.ToList())
        End Function

        ' GET: Returns/Details/5
        Function Details(ByVal id As Integer?) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim returnItem As ReturnItem = db.ReturnItems.Find(id)
            If IsNothing(returnItem) Then
                Return HttpNotFound()
            End If
            Return View(returnItem)
        End Function

        Function Search() As ActionResult
            ViewBag.returnMonth = DateTime.Now.AddMonths(1).ToString("yyyy年MM月")

            Dim categories = From c In db.Categories
                             Select New With {.CategoryId = c.CategoryId, .CategoryName = c.CategoryName}

            '第1引数:ドロップダウンリストの選択肢の中身（リスト）,第2引数:Value属性に割り当てされるもの（リストの主キーの列名）,
            '第3引数:Text属性に割り当てされるもの（リストの表示名の列名）,第4引数:初期値（予め選択されている項目）
            ViewBag.SelectCategory = New SelectList(categories, "CategoryId", "CategoryName", Nothing)


            ViewBag.SelectCategory2 = db.Categories.Select(Function(category) New SelectListItem With {
                                                             .Text = category.CategoryName,
                                                             .Value = category.CategoryId})

            '既にカート（返品明細テーブル）へ追加済みの商品の行数（数）を取得し、ViewBagへ格納
            ViewBag.returndetailsRowCount = db.ReturnItems.Where(Function(ri) ri.ReturnAmount = 0).Select(Function(ri) ri.ReturnDetails.Count()).FirstOrDefault()

            '既にカート（返品明細テーブル）へ追加済みの商品の行数（数）を取得し、ViewBagへ格納（上記コードの書き換え）
            'Dim returnid = db.ReturnItems.Where(Function(ri) ri.ReturnAmount = 0).Select(Function(ri) ri.ReturnId).FirstOrDefault()
            'ViewBag.returndetailsRowCount = db.ReturnDetails.Where(Function(returndetail) returndetail.ReturnId = returnid).Select(Function(returndetail) returndetail.ItemId).Count()

            Dim viewModel = New SearchViewModel()

            Return View(viewModel)
        End Function

        <HttpPost()>
        Function Search(<Bind(Include:="ItemId,CategoryId,ItemName")> ByVal model As SearchViewModel) As ActionResult
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

            'もし商品のカートへの追加が二回目以降の場合、返品番号が既に登録されているはずなので、その返品番号を取得する
            Dim returnid = db.ReturnItems.Where(Function(ri) ri.ReturnAmount = 0).Select(Function(ri) ri.ReturnId).FirstOrDefault()

            '既にカート（返品明細テーブル）へ追加済みの商品番号の取得
            Dim addedItemIdArray = db.ReturnDetails.Where(Function(returndetail) returndetail.ReturnId = returnid).Select(Function(returndetail) returndetail.ItemId)

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

            '検索ボタンをクリックすると、「ViewBag.returndetailsRowCount」が消えてしまうため、再度記述
            ViewBag.returndetailsRowCount = db.ReturnItems.Where(Function(ri) ri.ReturnAmount = 0).Select(Function(ri) ri.ReturnDetails.Count()).FirstOrDefault()

            Return View(model)
        End Function

        Function AddItem(ByVal id As Integer) As ActionResult
            'もし商品のカートへの追加が二回目以降の場合、返品番号が既に登録されているはずなので、その返品番号を取得する
            Dim returnid = db.ReturnItems.Where(Function(ri) ri.ReturnAmount = 0).Select(Function(ri) ri.ReturnId).FirstOrDefault()

            '上記で返品番号が作成されていない場合は、新しくReturnItemレコードを追加する
            If returnid = 0 Then
                '返品番号が返品明細テーブル登録時に必要なため、返品テーブルへ仮登録
                Dim result = db.ReturnItems.Add(New ReturnItem With {
                                   .ReturnedTime = DateTime.Now,
                                   .ReturnMonth = DirectCast(ViewBag.returnMonth, DateTime),
                                   .ReturnAmount = 0
                                   })
                db.SaveChanges()
                returnid = result.ReturnId
            End If

            '返品明細テーブルへitemIdを登録
            db.ReturnDetails.Add(New ReturnDetail With {
                               .ItemId = id,
                               .ReturnQuantity = 1,
                               .ReturnId = returnid
                               })

            db.SaveChanges()

            Return RedirectToAction("Create")
        End Function

        ' GET: Returns/Create
        Function Create() As ActionResult
            Dim categories = From c In db.Categories
                             Select New With {.CategoryId = c.CategoryId, .CategoryName = c.CategoryName}

            '第1引数:ドロップダウンリストの選択肢の中身（リスト）,第2引数:Value属性に割り当てされるもの（リストの主キーの列名）,
            '第3引数:Text属性に割り当てされるもの（リストの表示名の列名）,第4引数:初期値（予め選択されている項目）
            ViewBag.SelectCategory = New SelectList(categories, "CategoryId", "CategoryName", Nothing)

            Dim viewModel = New ReturnItem()

            'AjaxActionLinkでビューを表示する件がうまくいくまで、いったんコメントアウト
            viewModel = db.ReturnItems.Where(Function(ri) ri.ReturnAmount = 0).FirstOrDefault()

            Return View(viewModel)
        End Function

        ' POST: Returns/Create
        '過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        '詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 をご覧ください。
        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function Create(<Bind(Include:="ReturnId,ReturnDetails,ItemId,ItemName,UnitPrice,ReturnQuantity,CategoryName,ReturnedTime,ReturnMonth,ReturnAmount")> ByVal returnitem As ReturnItem, submit As String) As ActionResult
            'ReturnDetailsテーブルに商品が存在しない場合、そのまま画面に返す
            If returnitem.ReturnDetails Is Nothing Then
                If submit = "商品検索" Then

                    Return RedirectToAction("Search")

                ElseIf submit = "返品確定" Then

                    Return RedirectToAction("Index", "Items")

                End If
                Return View(returnitem)
            End If

            '返品金額を計算する
            Dim returnamount As Integer

            'For Eachによる合計の算出
            For Each returndetail In returnitem.ReturnDetails
                Dim unitprice = returndetail.Item.UnitPrice
                Dim returnquantity = returndetail.ReturnQuantity
                returnamount += unitprice * returnquantity
            Next

            'LINQのクエリ式による合計の算出
            'returnamount = (From returndetail In returnitem.ReturnDetails
            '                  Select returndetail.Item.UnitPrice * returndetail.ReturnQuantity).Sum()

            'LINQのメソッド式による合計の算出
            'returnamount = returnitem.ReturnDetails.Select(Function(returndetail) returndetail.Item.UnitPrice * returndetail.ReturnQuantity).Sum()

            '返品商品一覧画面で商品がある場合（削除して登録するものがない場合を除く）、ReturnDetailsテーブルに仮登録する
            For Each returndetail In returnitem.ReturnDetails
                '同じ商品を入れた際に保存エラーになるのを対策
                'db.Items.Attach(returndetail.Item)　※この記述ではうまくいかなかった
                returndetail.Item = Nothing '※ReturnDetailとItemとの関連を削除すると、正常に動作した

                db.Entry(returndetail).State = EntityState.Modified '※変更レコードとしてコンテキストに追加
            Next

            Dim now = DateTime.Now
            Dim yearmonth = New DateTime(now.Year, now.Month, 1).AddMonths(1)
            Dim InventoriesController As New InventoriesController

            If submit = "商品検索" Then

                'db.ReturnItems.Add(returnitem)　※ReturnItemsテーブルへの保存は今回行わない
                'db.ReturnDetails.AddRange(returnitem.ReturnDetails)　※Addは一行追加、AddRangeで複数行追加可能だが、行ごとに詳細設定が必要なためうまくいかなかった

                '変更をDBに保存する（ReturnDetailsテーブル）
                db.SaveChanges()

                Return RedirectToAction("Search")

            ElseIf submit = "返品確定" Then

                '未返品（返品テーブルの中で、返品金額が0で登録されている）の行のReturnIdを取得
                Dim returnid = db.ReturnItems.Where(Function(ri) ri.ReturnAmount = 0).Select(Function(ri) ri.ReturnId).FirstOrDefault()

                'ReturnItemsテーブルに合計金額を登録する
                Dim returnModel = db.ReturnItems.Single(Function(model) model.ReturnId = returnid)
                returnModel.ReturnAmount = returnamount

                '変更をDBに保存する（ReturnItemsテーブル、ReturnDetailsテーブル）
                db.SaveChanges()

                '在庫テーブルと発注テーブルの整合性を保つために在庫テーブルを更新する
                InventoriesController.Update(yearmonth)

                Return RedirectToAction("Index", "Items")

            ElseIf submit = "削除（非表示）" Then

                '返品商品一覧で削除フラグ（IsDelete）がTrueのものがあれば、行削除を行う
                For Each returndetail In returnitem.ReturnDetails

                    '商品番号を取得
                    Dim id = returndetail.ItemId

                    If IsNothing(id) Then
                        Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
                    End If

                    '未返品（返品テーブルの中で、返品金額が0で登録されている）の行のReturnIdを取得
                    Dim returnid = db.ReturnItems.Where(Function(r) r.ReturnAmount = 0).Select(Function(r) returnitem.ReturnId).FirstOrDefault()

                    Dim deletereturndetail As ReturnDetail = (From rd In db.ReturnDetails
                                                              Where rd.ItemId = id AndAlso rd.ReturnId = returnid AndAlso returndetail.IsDelete = True
                                                              Select rd).FirstOrDefault()

                    If Not deletereturndetail Is Nothing Then
                        db.ReturnDetails.Remove(deletereturndetail)
                    End If

                Next

                db.SaveChanges()

                '在庫テーブルと発注テーブルの整合性を保つために在庫テーブルを更新する
                InventoriesController.Update(yearmonth)

                Return RedirectToAction("Create")

            End If

            Return View(returnitem)
        End Function

        ' GET: Returns/Edit/5
        Function Edit(ByVal id As Integer?) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim returnItem As ReturnItem = db.ReturnItems.Find(id)
            If IsNothing(returnItem) Then
                Return HttpNotFound()
            End If
            Return View(returnItem)
        End Function

        ' POST: Returns/Edit/5
        '過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        '詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 をご覧ください。
        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function Edit(<Bind(Include:="ReturnId,ReturnedTime,ReturnMonth,ReturnAmount")> ByVal returnItem As ReturnItem) As ActionResult
            If ModelState.IsValid Then
                db.Entry(returnItem).State = EntityState.Modified
                db.SaveChanges()
                Return RedirectToAction("Index")
            End If
            Return View(returnItem)
        End Function

        Function Cancel() As ActionResult
            '未返品（返品テーブルの中で、返品金額が0で登録されている）の行のReturnIdを取得
            Dim returnid = db.ReturnItems.Where(Function(ri) ri.ReturnAmount = 0).Select(Function(ri) ri.ReturnId).FirstOrDefault()

            '上記で取得したReturnIdを持つ、返品テーブルのデータを削除　※紐づく返品明細テーブルの商品も自動的に削除される
            Dim returnData As ReturnItem = db.ReturnItems.Find(returnid)

            If Not returnData Is Nothing Then
                db.ReturnItems.Remove(returnData)
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
