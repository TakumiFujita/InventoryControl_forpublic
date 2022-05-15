@ModelType InventoryControl.Models.Inventory
@Code
    ViewData("Title") = "Delete"
End Code

<h2>Delete</h2>

<h3>Are you sure you want to delete this?</h3>
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
    @Using (Html.BeginForm())
        @Html.AntiForgeryToken()

        @<div class="form-actions no-color">
            <input type="submit" value="Delete" class="btn btn-default" /> |
            @Html.ActionLink("Back to List", "Index")
        </div>
    End Using
</div>
