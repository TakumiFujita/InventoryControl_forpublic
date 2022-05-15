@ModelType IEnumerable(Of InventoryControl.Models.InventoryListViewModel)
@Code
    ViewData("Title") = "Index"
End Code

<h2>在庫一覧</h2>
<p>
    @Html.ActionLink("商品登録", "Create", "Items", New With {.class = "btn btn-primary", .type = "button"})
    @Html.ActionLink("カテゴリー登録", "Create", New With {.controller = "Categories"}, New With {.class = "btn btn-primary", .type = "button"})
    @Html.ActionLink("発注", "Search", New With {.controller = "Orders"}, New With {.class = "btn btn-primary", .type = "button"})
    @Html.ActionLink("出荷", "Search", New With {.controller = "Shippings"}, New With {.class = "btn btn-primary", .type = "button"})
    @Html.ActionLink("返品", "Search", New With {.controller = "Returns"}, New With {.class = "btn btn-primary", .type = "button"})
</p>
<div>
    @Html.DropDownList("selectMonth", DirectCast(ViewBag.Month, List(Of SelectListItem)), New With {.class = "btn btn-default dropdown-toggle"})
</div>
<table class="table">
    <tr>
        <th>
            @Html.DisplayNameFor(Function(model) model.ItemId)
        </th>
        <th>
            @Html.DisplayNameFor(Function(model) model.ItemName)
        </th>
        <th>
            @Html.DisplayNameFor(Function(model) model.CategoryName)
        </th>
        <th>
            @Html.DisplayNameFor(Function(model) model.LastMonthsStockQuantity)
        </th>
        <th>
            @Html.DisplayNameFor(Function(model) model.OrderQuantity)
        </th>
        <th>
            @Html.DisplayNameFor(Function(model) model.ShippingQuantity)
        </th>
        <th>
            @Html.DisplayNameFor(Function(model) model.ReturnQuantity)
        </th>
        <th>
            @Html.DisplayNameFor(Function(model) model.StockQuantity)
        </th>
    </tr>
    @For Each item In Model
        @<tr>
            <td>
                @Html.DisplayFor(Function(modelItem) item.ItemId)
            </td>
            <td>
                @Html.DisplayFor(Function(modelItem) item.ItemName)
            </td>
            <td>
                @Html.DisplayFor(Function(modelItem) item.CategoryName)
            </td>
            <td>
                @Html.DisplayFor(Function(modelItem) item.LastMonthsStockQuantity)
            </td>
            <td>
                @Html.DisplayFor(Function(modelItem) item.OrderQuantity)
            </td>
            <td>
                @Html.DisplayFor(Function(modelItem) item.ShippingQuantity)
            </td>
            <td>
                @Html.DisplayFor(Function(modelItem) item.ReturnQuantity)
            </td>
            <td>
                @Html.DisplayFor(Function(modelItem) item.StockQuantity)
            </td>
        </tr>
    Next
</table>

@Section Scripts
    @Scripts.Render("~/bundles/jqueryval")
<script>
        $(function () {
            $('#selectMonth').change(function (e) {
                var url = '@Url.Action("Index", "Items")';
                window.location.href = url + "?value=" + e.target.value;
            });
        });
</script>
End Section
