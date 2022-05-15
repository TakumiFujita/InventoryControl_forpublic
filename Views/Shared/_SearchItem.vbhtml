@ModelType InventoryControl.Models.SearchViewModel

@*モーダルウィンドウ*@
<div id="SearchItemModal" class="modal" data-backdrop="static">
    <div class="modal-dialog modal-dialog-scrollable modal-lg">
        <div class="modal-content">
            <div class="modal-header">
                <h4>商品検索</h4>
            </div>
            <div class="modal-body">

                @Using (Html.BeginForm("SearchItem", "Orders", Nothing, FormMethod.Post, New With {.enctype = "multipart/form-data"}))
                    @Html.AntiForgeryToken()

                    @<div class="form-horizontal">
                        @Html.ValidationSummary(True, "", New With {.class = "text-danger"})
                        <div class="form-group">
                            @Html.LabelFor(Function(model) model.ItemId, htmlAttributes:=New With {.class = "control-label col-md-2"})
                            <div class="col-md-10">
                                @Html.EditorFor(Function(model) model.ItemId, New With {.htmlAttributes = New With {.class = "form-control tBoxWidth"}})
                            </div>
                        </div>

                        @*<div class="form-group">
                            @Html.LabelFor(Function(model) model.CategoryName, htmlAttributes:=New With {.class = "control-label col-md-2"})
                            <div class="col-md-10">
                                @Html.DropDownListFor(Function(model) model.CategoryName, DirectCast(ViewBag.SelectCategory, SelectList), "選択してください", New With {.class = "form-control"})
                            </div>
                        </div>*@

                        <div class="form-group">
                            @Html.LabelFor(Function(model) model.ItemName, htmlAttributes:=New With {.class = "control-label col-md-2"})
                            <div class="col-md-10">
                                @Html.EditorFor(Function(model) model.ItemName, New With {.htmlAttributes = New With {.class = "form-control tBoxWidth"}})
                            </div>
                        </div>

                        <div Class="form-group">
                            <div Class="col-md-offset-2 col-md-10">
                                <input type="submit" value="検索" class="btn btn-default" />
                            </div>
                            <div class="col-md-offset-2 col-md-10">
                                <input type="button" id="BtnSearch" Class="btn btn-info" value="search" />
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
                        @*<th>
                    @Html.DisplayNameFor(Function(model) model.CategoryName)
                </th>*@
                        <th>
                            @Html.DisplayNameFor(Function(model) model.ItemName)
                        </th>
                        <th>
                            @Html.DisplayNameFor(Function(model) model.UnitPrice)
                        </th>
                        <th></th>
                    </tr>

                    @For Each item In Model.Items
                        @<tr>
                            <td>
                                @Html.DisplayFor(Function(modelItem) item.ItemId)
                            </td>
                            <td>
                                @Html.DisplayFor(Function(modelItem) item.ItemName)
                            </td>
                            <td>
                                @Html.DisplayFor(Function(modelItem) item.UnitPrice)
                            </td>
                        </tr>
                    Next
                </table>
            </div>

            </div>
            <div Class="modal-footer">
                <Button type="button" Class="btn btn-secondary" data-dismiss="modal">閉じる</Button>
                <Button type="button" Class="btn btn-primary">商品追加</Button>
            </div>
        </div>
    </div>
</div>

@*<div>
        @Html.ActionLink("Back to List", "Index")
    </div>*@