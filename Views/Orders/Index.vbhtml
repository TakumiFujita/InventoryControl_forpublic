@ModelType IEnumerable(Of InventoryControl.Models.Order)
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
            @Html.DisplayNameFor(Function(model) model.OrderedTime)
        </th>
        <th>
            @Html.DisplayNameFor(Function(model) model.OrderMonth)
        </th>
        <th>
            @Html.DisplayNameFor(Function(model) model.OrderAmount)
        </th>
        <th></th>
    </tr>

@For Each item In Model
    @<tr>
        <td>
            @Html.DisplayFor(Function(modelItem) item.OrderedTime)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) item.OrderMonth)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) item.OrderAmount)
        </td>
        <td>
            @Html.ActionLink("Edit", "Edit", New With {.id = item.OrderId }) |
            @Html.ActionLink("Details", "Details", New With {.id = item.OrderId }) |
            @Html.ActionLink("Delete", "Delete", New With {.id = item.OrderId })
        </td>
    </tr>
Next

</table>
