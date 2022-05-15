@ModelType InventoryControl.Models.Order
@Code
    ViewData("Title") = "Delete"
End Code

<h2>Delete</h2>

<h3>Are you sure you want to delete this?</h3>
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
    @Using (Html.BeginForm())
        @Html.AntiForgeryToken()

        @<div class="form-actions no-color">
            <input type="submit" value="Delete" class="btn btn-default" /> |
            @Html.ActionLink("Back to List", "Index")
        </div>
    End Using
</div>
