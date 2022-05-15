@ModelType InventoryControl.Models.ReturnItem
@Code
    ViewData("Title") = "Details"
End Code

<h2>Details</h2>

<div>
    <h4>ReturnItem</h4>
    <hr />
    <dl class="dl-horizontal">
        <dt>
            @Html.DisplayNameFor(Function(model) model.ReturnedTime)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.ReturnedTime)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.ReturnMonth)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.ReturnMonth)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.ReturnAmount)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.ReturnAmount)
        </dd>

    </dl>
</div>
<p>
    @Html.ActionLink("Edit", "Edit", New With { .id = Model.ReturnId }) |
    @Html.ActionLink("Back to List", "Index")
</p>
