@ModelType InventoryControl.Models.SearchViewModel

@Code
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<div>
    <div class="modal-header">
        <h2>商品検索</h2>
    </div>
    <div class="modal-body">

        @Using (Html.BeginForm("Search", "Returns", Nothing, FormMethod.Post, New With {.enctype = "multipart/form-data"}))
            @Html.AntiForgeryToken()

            @<div class="form-horizontal">
                @Html.ValidationSummary(True, "", New With {.class = "text-danger"})
                <div class="form-group">
                    @Html.LabelFor(Function(model) model.ItemId, htmlAttributes:=New With {.class = "control-label col-md-2"})
                    <div class="col-md-10">
                        @Html.EditorFor(Function(model) model.ItemId, New With {.htmlAttributes = New With {.class = "form-control tBoxWidth"}})
                    </div>
                </div>

                <div class="form-group">
                    @Html.LabelFor(Function(model) model.CategoryName, htmlAttributes:=New With {.class = "control-label col-md-2"})
                    <div class="col-md-10">
                        @Html.DropDownListFor(Function(model) model.CategoryId, DirectCast(ViewBag.SelectCategory2, IEnumerable(Of SelectListItem)), "選択してください", New With {.class = "form-control"})
                    </div>
                </div>

                <div class="form-group">
                    @Html.LabelFor(Function(model) model.ItemName, htmlAttributes:=New With {.class = "control-label col-md-2"})
                    <div class="col-md-10">
                        @Html.EditorFor(Function(model) model.ItemName, New With {.htmlAttributes = New With {.class = "form-control tBoxWidth"}})
                    </div>
                </div>

                <div Class="form-group">
                    <div class="col-md-offset-2 col-md-10">
                        <input type="submit" value="検索" class="btn btn-default" />
                    </div>
                </div>
            </div>
        End Using

        <div id="SearchResult">
            <table Class="table">
                <tr>
                    <th>
                        @Html.DisplayNameFor(Function(model) model.ItemId)
                    </th>
                    <th>
                        @Html.DisplayNameFor(Function(model) model.CategoryName)
                    </th>
                    <th>
                        @Html.DisplayNameFor(Function(model) model.ItemName)
                    </th>
                    <th>
                        @Html.DisplayNameFor(Function(model) model.UnitPrice)
                    </th>
                    <th></th>
                    <th></th>
                </tr>

                @For Each item In Model.Items
                    @Using (Html.BeginForm("AddItem", "Returns", Nothing, FormMethod.Post))
                        @<tr>
                            <td>
                                @Html.DisplayFor(Function(modelItem) item.ItemId)
                            </td>
                            <td>
                                @Html.DisplayFor(Function(modelItem) item.CategoryName)
                            </td>
                            <td>
                                @Html.DisplayFor(Function(modelItem) item.ItemName)
                            </td>
                            <td>
                                @Html.DisplayFor(Function(modelItem) item.UnitPrice)
                            </td>
                            <td>
                                @*商品がカート（返品明細テーブル）に入っていなければ、「追加」ボタンを表示する*@
                                @If item.IsSelectable Then
                                    @Html.ActionLink("商品を追加", "AddItem", New With {.id = item.ItemId}, New With {.class = "btn btn-default", .type = "button"})
                                Else
                                    @<input type="button" value="追加済み" class="btn btn-default" disabled />
                                End If
                            </td>
                        </tr>
                    End Using
                Next
            </table>
        </div>
    </div>

    <div Class="modal-footer">
        @If Not ViewBag.returndetailsRowCount = 0 Then
            @Html.ActionLink("返品商品一覧", "Create", New With {.controller = "Returns"}, New With {.class = "btn btn-default", .type = "button"})
        End If
        @Html.ActionLink("在庫一覧へ戻る", "Cancel", New With {.controller = "Returns"}, New With {.class = "btn btn-default", .type = "button", .id = "deleteConfirm"})
    </div>
</div>

@Section Scripts
    <script src="~/Scripts/Returns/search.js"></script>
End Section