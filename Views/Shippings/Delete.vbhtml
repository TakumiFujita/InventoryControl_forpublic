@ModelType InventoryControl.Models.Shipping
@Code
    ViewData("Title") = "Delete"
End Code

<h2>Delete</h2>

<h3>Are you sure you want to delete this?</h3>
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
    @Using (Html.BeginForm())
        @Html.AntiForgeryToken()

        @<div class="form-actions no-color">
            <input type="submit" value="Delete" class="btn btn-default" /> |
            @Html.ActionLink("Back to List", "Index")
        </div>
    End Using
</div>
