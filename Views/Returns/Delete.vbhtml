@ModelType InventoryControl.Models.ReturnItem
@Code
    ViewData("Title") = "Delete"
End Code

<h2>Delete</h2>

<h3>Are you sure you want to delete this?</h3>
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
    @Using (Html.BeginForm())
        @Html.AntiForgeryToken()

        @<div class="form-actions no-color">
            <input type="submit" value="Delete" class="btn btn-default" /> |
            @Html.ActionLink("Back to List", "Index")
        </div>
    End Using
</div>
