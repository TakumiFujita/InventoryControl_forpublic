@ModelType IEnumerable(Of InventoryControl.Models.Category)
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
            @Html.DisplayNameFor(Function(model) model.CategoryName)
        </th>
        <th></th>
    </tr>

@For Each item In Model
    @<tr>
        <td>
            @Html.DisplayFor(Function(modelItem) item.CategoryName)
        </td>
        <td>
            @Html.ActionLink("Edit", "Edit", New With {.id = item.CategoryId }) |
            @Html.ActionLink("Details", "Details", New With {.id = item.CategoryId }) |
            @Html.ActionLink("Delete", "Delete", New With {.id = item.CategoryId })
        </td>
    </tr>
Next

</table>
