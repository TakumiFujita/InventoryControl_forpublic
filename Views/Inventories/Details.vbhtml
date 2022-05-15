@ModelType InventoryControl.Models.Inventory
@Code
    ViewData("Title") = "Details"
End Code

<h2>Details</h2>

<div>
    <h4>Inventory</h4>
    <hr />
    <dl class="dl-horizontal">
        <dt>
            @Html.DisplayNameFor(Function(model) model.Item.ItemName)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.Item.ItemName)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.YearMonth)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.YearMonth)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.LastMonthsStockQuantity)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.LastMonthsStockQuantity)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.OrderQuantity)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.OrderQuantity)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.ShippingQuantity)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.ShippingQuantity)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.ReturnQuantity)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.ReturnQuantity)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.StockQuantity)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.StockQuantity)
        </dd>

    </dl>
</div>
<p>
    @Html.ActionLink("Edit", "Edit", New With { .id = Model.InventoryDataId }) |
    @Html.ActionLink("Back to List", "Index")
</p>
