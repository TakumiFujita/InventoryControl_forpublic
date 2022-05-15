@ModelType InventoryControl.Models.Shipping
@Code
    ViewData("Title") = "Details"
End Code

<h2>Details</h2>

<div>
    <h4>Shipping</h4>
    <hr />
    <dl class="dl-horizontal">
        <dt>
            @Html.DisplayNameFor(Function(model) model.ShippedTime)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.ShippedTime)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.ShippingMonth)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.ShippingMonth)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.ShippingAmount)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.ShippingAmount)
        </dd>

    </dl>
</div>
<p>
    @Html.ActionLink("Edit", "Edit", New With { .id = Model.ShippingId }) |
    @Html.ActionLink("Back to List", "Index")
</p>
