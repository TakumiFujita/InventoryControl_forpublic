@ModelType InventoryControl.Models.Item
@Code
    ViewData("Title") = "Create"
End Code

<h2>商品登録</h2>

@Using (Html.BeginForm())
    @Html.AntiForgeryToken()

    @<div class="form-horizontal">
        @*<h4>Item</h4>*@
        <hr />
        @Html.ValidationSummary(True, "", New With {.class = "text-danger"})
        <div class="form-group">
            @Html.LabelFor(Function(model) model.ItemId, htmlAttributes:=New With {.class = "control-label col-md-2"})
            <div class="col-md-10">
                @Html.EditorFor(Function(model) model.ItemId, New With {.htmlAttributes = New With {.class = "form-control"}})
                @Html.ValidationMessageFor(Function(model) model.ItemId, "", New With {.class = "text-danger"})
                @Html.ValidationMessage("EM_DuplicateItemId", New With {.class = "text-danger"})
            </div>
        </div>

        <div class="form-group">
            @Html.LabelFor(Function(model) model.ItemName, htmlAttributes:=New With {.class = "control-label col-md-2"})
            <div class="col-md-10">
                @Html.EditorFor(Function(model) model.ItemName, New With {.htmlAttributes = New With {.class = "form-control"}})
                @Html.ValidationMessageFor(Function(model) model.ItemName, "", New With {.class = "text-danger"})
                @Html.ValidationMessage("EM_DuplicateItemName", New With {.class = "text-danger"})
            </div>
        </div>

        @*<div class="form-group">
                @Html.LabelFor(Function(model) model.CategoryId, htmlAttributes:=New With {.class = "control-label col-md-2"})
                <div class="col-md-10">
                    @Html.EditorFor(Function(model) model.CategoryId, New With {.htmlAttributes = New With {.class = "form-control"}})
                    @Html.ValidationMessageFor(Function(model) model.CategoryId, "", New With {.class = "text-danger"})
                </div>
            </div>*@

        <div class="form-group">
            @Html.LabelFor(Function(model) model.Category.CategoryName, htmlAttributes:=New With {.class = "control-label col-md-2"})
            <div class="col-md-10">
                @Html.DropDownListFor(Function(model) model.CategoryId, DirectCast(ViewBag.SelectCategory, SelectList), "選択してください", New With {.class = "form-control"})
                @*以下の書き方でもOK*@
                @*@Html.DropDownListFor(Function(model) model.CategoryId, DirectCast(ViewBag.SelectCategory, IEnumerable(Of SelectListItem)), "選択してください", New With {.class = "form-control"})*@
                @Html.ValidationMessageFor(Function(model) model.CategoryId, "", New With {.class = "text-danger"})
            </div>
        </div>

        <div Class="form-group">
            @Html.LabelFor(Function(model) model.UnitPrice, htmlAttributes:=New With {.class = "control-label col-md-2"})
            <div class="col-md-10">
                @Html.EditorFor(Function(model) model.UnitPrice, New With {.htmlAttributes = New With {.class = "form-control"}})
                @Html.ValidationMessageFor(Function(model) model.UnitPrice, "", New With {.class = "text-danger"})
            </div>
        </div>

        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <input type="submit" value="登録" class="btn btn-primary" />
                @Html.ActionLink("戻る", "Index", "Items", New With {.class = "btn btn-default", .type = "button"})
            </div>
        </div>
    </div>
End Using

@Section Scripts
    @Scripts.Render("~/bundles/jqueryval")
End Section
