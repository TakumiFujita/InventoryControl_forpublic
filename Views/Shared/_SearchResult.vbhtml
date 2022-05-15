@ModelType InventoryControl.Models.SearchViewModel

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
