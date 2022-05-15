@ModelType InventoryControl.Models.SearchViewModel

@Code
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

@*<div id="SearchItemModal" class="modal" data-backdrop="static">*@
@*<div class="modal-dialog modal-dialog-scrollable modal-lg">*@
<div>
    <div class="modal-header">
        <h2>商品検索</h2>
    </div>
    <div class="modal-body">

        @Using (Html.BeginForm("Search", "Orders", Nothing, FormMethod.Post, New With {.enctype = "multipart/form-data"}))
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
                    @Using (Html.BeginForm("AddItem", "Orders", Nothing, FormMethod.Post))
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
                                @*商品がカート（発注明細テーブル）に入っていなければ、「追加」ボタンを表示する*@
                                @If item.IsSelectable Then
                                    @Html.ActionLink("商品を追加", "AddItem", New With {.id = item.ItemId}, New With {.class = "btn btn-default", .type = "button"})
                                Else
                                    @<input type="button" value="追加済み" class="btn btn-default" disabled />
                                End If

                                @*いったん保留*@
                                @*@Ajax.ActionLink("商品を追加", "AddItem", New With {.id = item.ItemId}, New AjaxOptions With {.HttpMethod = "Post"}, New With {.class = "btn btn-default"})*@
                                @*<input type="submit" id="@Html.IdFor(Function(a) item.ItemId)" value="商品を追加" class="btn btn-default" />*@
                                @*@Ajax.ActionLink("ToAddItem", "AddItem", New With {.id = item.ItemId}, New AjaxOptions With {.HttpMethod = "Post"}, New With {.ItemId = item.ItemId, .CategoryId = item.CategoryId, .ItemName = item.ItemName})*@
                                @*<input type="submit" value="追加" name="item" class="btn btn-default" />*@
                            </td>
                        </tr>
                    End Using
                Next
            </table>
        </div>
    </div>

    <div Class="modal-footer">
        @*<Button type="button" Class="btn btn-secondary" data-dismiss="modal">閉じる</Button>*@
        @If Not ViewBag.orderdetailsRowCount = 0 Then
            @Html.ActionLink("発注商品一覧", "Create", New With {.controller = "Orders"}, New With {.class = "btn btn-default", .type = "button"})
        End If
        @Html.ActionLink("在庫一覧へ戻る", "Cancel", New With {.controller = "Orders"}, New With {.class = "btn btn-default", .type = "button", .id = "deleteConfirm"})
    </div>
</div>
@*</div>*@
@*</div>*@

@Section Scripts
    <script src="~/Scripts/Orders/search.js"></script>
End Section
