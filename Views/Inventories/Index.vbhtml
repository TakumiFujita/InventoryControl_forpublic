@ModelType IEnumerable(Of InventoryControl.Models.Inventory)
@Code
ViewData("Title") = "Index"
End Code

<h2>Index</h2>

<p>
    @Html.ActionLink("Create New", "Create")
</p>
<table class="table">
    <tr>
        <th>
            @Html.DisplayNameFor(Function(model) model.Item.ItemName)
        </th>
        <th>
            @Html.DisplayNameFor(Function(model) model.YearMonth)
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
        <th></th>
    </tr>

@For Each item In Model
    @<tr>
        <td>
            @Html.DisplayFor(Function(modelItem) item.Item.ItemName)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) item.YearMonth)
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
        <td>
            @Html.ActionLink("Edit", "Edit", New With {.id = item.InventoryDataId }) |
            @Html.ActionLink("Details", "Details", New With {.id = item.InventoryDataId }) |
            @Html.ActionLink("Delete", "Delete", New With {.id = item.InventoryDataId })
        </td>
    </tr>
Next

</table>
