@ModelType IEnumerable(Of InventoryControl.Models.ReturnItem)
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
            @Html.DisplayNameFor(Function(model) model.ReturnedTime)
        </th>
        <th>
            @Html.DisplayNameFor(Function(model) model.ReturnMonth)
        </th>
        <th>
            @Html.DisplayNameFor(Function(model) model.ReturnAmount)
        </th>
        <th></th>
    </tr>

@For Each item In Model
    @<tr>
        <td>
            @Html.DisplayFor(Function(modelItem) item.ReturnedTime)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) item.ReturnMonth)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) item.ReturnAmount)
        </td>
        <td>
            @Html.ActionLink("Edit", "Edit", New With {.id = item.ReturnId }) |
            @Html.ActionLink("Details", "Details", New With {.id = item.ReturnId }) |
            @Html.ActionLink("Delete", "Delete", New With {.id = item.ReturnId })
        </td>
    </tr>
Next

</table>
