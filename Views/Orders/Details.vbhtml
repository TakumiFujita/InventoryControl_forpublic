@ModelType InventoryControl.Models.Order
@Code
    ViewData("Title") = "Details"
End Code

<h2>Details</h2>

<div>
    <h4>Order</h4>
    <hr />
    <dl class="dl-horizontal">
        <dt>
            @Html.DisplayNameFor(Function(model) model.OrderedTime)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.OrderedTime)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.OrderMonth)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.OrderMonth)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.OrderAmount)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.OrderAmount)
        </dd>

    </dl>
</div>
<p>
    @Html.ActionLink("Edit", "Edit", New With { .id = Model.OrderId }) |
    @Html.ActionLink("Back to List", "Index")
</p>
