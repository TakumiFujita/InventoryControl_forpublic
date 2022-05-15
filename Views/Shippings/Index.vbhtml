@ModelType IEnumerable(Of InventoryControl.Models.Shipping)
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
            @Html.DisplayNameFor(Function(model) model.ShippedTime)
        </th>
        <th>
            @Html.DisplayNameFor(Function(model) model.ShippingMonth)
        </th>
        <th>
            @Html.DisplayNameFor(Function(model) model.ShippingAmount)
        </th>
        <th></th>
    </tr>

@For Each item In Model
    @<tr>
        <td>
            @Html.DisplayFor(Function(modelItem) item.ShippedTime)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) item.ShippingMonth)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) item.ShippingAmount)
        </td>
        <td>
            @Html.ActionLink("Edit", "Edit", New With {.id = item.ShippingId }) |
            @Html.ActionLink("Details", "Details", New With {.id = item.ShippingId }) |
            @Html.ActionLink("Delete", "Delete", New With {.id = item.ShippingId })
        </td>
    </tr>
Next

</table>
