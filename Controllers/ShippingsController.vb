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
    Public Class ShippingsController
        Inherits System.Web.Mvc.Controller

        Private db As New InventoryControlContext

        Public Sub New()
            Dim now = DateTime.Now
            Dim shippingMonth = New DateTime(now.Year, now.Month, 1).AddMonths(1)

            ViewBag.shippingMonth = shippingMonth
            ViewBag.displayShippingMonth = shippingMonth.ToString("yyyy年MM月")
        End Sub

        ' GET: Shippings
        Function Index() As ActionResult
            Return View(db.Shippings.ToList())
        End Function

        ' GET: Shippings/Details/5
        Function Details(ByVal id As Integer?) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim shipping As Shipping = db.Shippings.Find(id)
            If IsNothing(shipping) Then
                Return HttpNotFound()
            End If
            Return View(shipping)
        End Function

        Function Search() As ActionResult
            ViewBag.shippingMonth = DateTime.Now.AddMonths(1).ToString("yyyy年MM月")

            Dim categories = From c In db.Categories
                             Select New With {.CategoryId = c.CategoryId, .CategoryName = c.CategoryName}

            '第1引数:ドロップダウンリストの選択肢の中身（リスト）,第2引数:Value属性に割り当てされるもの（リストの主キーの列名）,
            '第3引数:Text属性に割り当てされるもの（リストの表示名の列名）,第4引数:初期値（予め選択されている項目）
            ViewBag.SelectCategory = New SelectList(categories, "CategoryId", "CategoryName", Nothing)


            ViewBag.SelectCategory2 = db.Categories.Select(Function(category) New SelectListItem With {
                                                             .Text = category.CategoryName,
                                                             .Value = category.CategoryId})

            '既にカート（出荷明細テーブル）へ追加済みの商品の行数（数）を取得し、ViewBagへ格納
            ViewBag.shippingdetailsRowCount = db.Shippings.Where(Function(shipping) shipping.ShippingAmount = 0).Select(Function(shipping) shipping.ShippingDetails.Count()).FirstOrDefault()

            '既にカート（出荷明細テーブル）へ追加済みの商品の行数（数）を取得し、ViewBagへ格納（上記コードの書き換え）
            'Dim shippingid = db.Shippings.Where(Function(shipping) shipping.ShippingAmount = 0).Select(Function(shipping) shipping.ShippingId).FirstOrDefault()
            'ViewBag.shippingdetailsRowCount = db.ShippingDetails.Where(Function(shippingdetail) shippingdetail.ShippingId = shippingid).Select(Function(shippingdetail) shippingdetail.ItemId).Count()

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

            'もし商品のカートへの追加が二回目以降の場合、出荷番号が既に登録されているはずなので、その出荷番号を取得する
            Dim shippingid = db.Shippings.Where(Function(shipping) shipping.ShippingAmount = 0).Select(Function(shipping) shipping.ShippingId).FirstOrDefault()

            '既にカート（出荷明細テーブル）へ追加済みの商品番号の取得
            Dim addedItemIdArray = db.ShippingDetails.Where(Function(shippingdetail) shippingdetail.ShippingId = shippingid).Select(Function(shippingdetail) shippingdetail.ItemId)

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

            '検索ボタンをクリックすると、「ViewBag.shippingdetailsRowCount」が消えてしまうため、再度記述
            ViewBag.shippingdetailsRowCount = db.Shippings.Where(Function(shipping) shipping.ShippingAmount = 0).Select(Function(shipping) shipping.ShippingDetails.Count()).FirstOrDefault()

            Return View(model)
        End Function

        Function AddItem(ByVal id As Integer) As ActionResult
            'もし商品のカートへの追加が二回目以降の場合、出荷番号が既に登録されているはずなので、その出荷番号を取得する
            Dim shippingid = db.Shippings.Where(Function(shipping) shipping.ShippingAmount = 0).Select(Function(shipping) shipping.ShippingId).FirstOrDefault()

            '上記で出荷番号が作成されていない場合は、新しくShippingレコードを追加する
            If shippingid = 0 Then
                '出荷番号が出荷明細テーブル登録時に必要なため、出荷テーブルへ仮登録
                Dim result = db.Shippings.Add(New Shipping With {
                                   .ShippedTime = DateTime.Now,
                                   .ShippingMonth = DirectCast(ViewBag.shippingMonth, DateTime),
                                   .ShippingAmount = 0
                                   })
                db.SaveChanges()
                shippingid = result.ShippingId
            End If

            '出荷明細テーブルへitemIdを登録
            db.ShippingDetails.Add(New ShippingDetail With {
                               .ItemId = id,
                               .ShippingQuantity = 1,
                               .ShippingId = shippingid
                               })

            db.SaveChanges()

            Return RedirectToAction("Create")
        End Function

        ' GET: Shippings/Create
        Function Create() As ActionResult
            Dim categories = From c In db.Categories
                             Select New With {.CategoryId = c.CategoryId, .CategoryName = c.CategoryName}

            '第1引数:ドロップダウンリストの選択肢の中身（リスト）,第2引数:Value属性に割り当てされるもの（リストの主キーの列名）,
            '第3引数:Text属性に割り当てされるもの（リストの表示名の列名）,第4引数:初期値（予め選択されている項目）
            ViewBag.SelectCategory = New SelectList(categories, "CategoryId", "CategoryName", Nothing)

            Dim viewModel = New Shipping()

            'AjaxActionLinkでビューを表示する件がうまくいくまで、いったんコメントアウト
            viewModel = db.Shippings.Where(Function(shipping) shipping.ShippingAmount = 0).FirstOrDefault()

            Return View(viewModel)
        End Function

        ' POST: Shippings/Create
        '過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        '詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 をご覧ください。
        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function Create(<Bind(Include:="ShippingId,ShippingDetails,ItemId,ItemName,UnitPrice,ShippingQuantity,CategoryName,ShippedTime,ShippingMonth,ShippingAmount")> ByVal shipping As Shipping, submit As String) As ActionResult

            'ShippingDetailsテーブルに商品が存在しない場合、そのまま画面に返す
            If shipping.ShippingDetails Is Nothing Then
                If submit = "商品検索" Then

                    Return RedirectToAction("Search")

                ElseIf submit = "出荷確定" Then

                    Return RedirectToAction("Index", "Items")

                End If
                Return View(shipping)
            End If

            '出荷金額を計算する
            Dim shippingamount As Integer

            'For Eachによる合計の算出
            For Each shippingdetail In shipping.ShippingDetails
                Dim unitprice = shippingdetail.Item.UnitPrice
                Dim shippingquantity = shippingdetail.ShippingQuantity
                shippingamount += unitprice * shippingquantity
            Next

            'LINQのクエリ式による合計の算出
            'shippingamount = (From shippingdetail In shipping.ShippingDetails
            '                  Select shippingdetail.Item.UnitPrice * shippingdetail.ShippingQuantity).Sum()

            'LINQのメソッド式による合計の算出
            'shippingamount = shipping.ShippingDetails.Select(Function(shippingdetail) shippingdetail.Item.UnitPrice * shippingdetail.ShippingQuantity).Sum()

            '出荷商品一覧画面で商品がある場合（削除して登録するものがない場合を除く）、ShippingDetailsテーブルに仮登録する
            For Each shippingdetail In shipping.ShippingDetails
                '同じ商品を入れた際に保存エラーになるのを対策
                'db.Items.Attach(shippingdetail.Item)　※この記述ではうまくいかなかった
                shippingdetail.Item = Nothing '※ShippingDetailとItemとの関連を削除すると、正常に動作した

                db.Entry(shippingdetail).State = EntityState.Modified '※変更レコードとしてコンテキストに追加
            Next

            Dim now = DateTime.Now
            Dim yearmonth = New DateTime(now.Year, now.Month, 1).AddMonths(1)
            Dim InventoriesController As New InventoriesController

            If submit = "商品検索" Then

                'db.Shippings.Add(shipping)　※Shippingsテーブルへの保存は今回行わない
                'db.ShippingDetails.AddRange(shipping.ShippingDetails)　※Addは一行追加、AddRangeで複数行追加可能だが、行ごとに詳細設定が必要なためうまくいかなかった

                '変更をDBに保存する（ShippingDetailsテーブル）
                db.SaveChanges()

                Return RedirectToAction("Search")

            ElseIf submit = "出荷確定" Then

                '未出荷（出荷テーブルの中で、出荷金額が0で登録されている）の行のShippingIdを取得
                Dim shippingid = db.Shippings.Where(Function(s) s.ShippingAmount = 0).Select(Function(s) s.ShippingId).FirstOrDefault()

                'Shippingsテーブルに合計金額を登録する
                Dim shippingModel = db.Shippings.Single(Function(model) model.ShippingId = shippingid)
                shippingModel.ShippingAmount = shippingamount

                '変更をDBに保存する（Shippingsテーブル、ShippingDetailsテーブル）
                db.SaveChanges()

                '在庫テーブルと発注テーブルの整合性を保つために在庫テーブルを更新する
                InventoriesController.Update(yearmonth)

                Return RedirectToAction("Index", "Items")

            ElseIf submit = "削除（非表示）" Then

                '出荷商品一覧で削除フラグ（IsDelete）がTrueのものがあれば、行削除を行う
                For Each shippingdetail In shipping.ShippingDetails

                    '商品番号を取得
                    Dim id = shippingdetail.ItemId

                    If IsNothing(id) Then
                        Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
                    End If

                    '未出荷（出荷テーブルの中で、出荷金額が0で登録されている）の行のShippingIdを取得
                    Dim shippingid = db.Shippings.Where(Function(s) s.ShippingAmount = 0).Select(Function(s) shipping.ShippingId).FirstOrDefault()

                    Dim deleteshippingdetail As ShippingDetail = (From sd In db.ShippingDetails
                                                                  Where sd.ItemId = id AndAlso sd.ShippingId = shippingid AndAlso shippingdetail.IsDelete = True
                                                                  Select sd).FirstOrDefault()

                    If Not deleteshippingdetail Is Nothing Then
                        db.ShippingDetails.Remove(deleteshippingdetail)
                    End If

                Next

                db.SaveChanges()

                '在庫テーブルと発注テーブルの整合性を保つために在庫テーブルを更新する
                InventoriesController.Update(yearmonth)

                Return RedirectToAction("Create")

            End If

            Return View(shipping)
        End Function

        ' GET: Shippings/Edit/5
        Function Edit(ByVal id As Integer?) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim shipping As Shipping = db.Shippings.Find(id)
            If IsNothing(shipping) Then
                Return HttpNotFound()
            End If
            Return View(shipping)
        End Function

        ' POST: Shippings/Edit/5
        '過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        '詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 をご覧ください。
        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function Edit(<Bind(Include:="ShippingId,ShippedTime,ShippingMonth,ShippingAmount")> ByVal shipping As Shipping) As ActionResult
            If ModelState.IsValid Then
                db.Entry(shipping).State = EntityState.Modified
                db.SaveChanges()
                Return RedirectToAction("Index")
            End If
            Return View(shipping)
        End Function

        Function Cancel() As ActionResult
            '未出荷（出荷テーブルの中で、出荷金額が0で登録されている）の行のShippingIdを取得
            Dim shippingid = db.Shippings.Where(Function(shipping) shipping.ShippingAmount = 0).Select(Function(shipping) shipping.ShippingId).FirstOrDefault()

            '上記で取得したShippingIdを持つ、出荷テーブルのデータを削除　※紐づく出荷明細テーブルの商品も自動的に削除される
            Dim shippingData As Shipping = db.Shippings.Find(shippingid)

            If Not shippingData Is Nothing Then
                db.Shippings.Remove(shippingData)
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
