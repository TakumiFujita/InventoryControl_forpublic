@ModelType InventoryControl.Models.Category
@Code
    ViewData("Title") = "Create"
End Code

<h2>カテゴリー登録</h2>

@Using (Html.BeginForm()) 
    @Html.AntiForgeryToken()
    
    @<div class="form-horizontal">
        <hr />
        @Html.ValidationSummary(True, "", New With { .class = "text-danger" })
        <div class="form-group">
            @Html.LabelFor(Function(model) model.CategoryId, htmlAttributes:= New With { .class = "control-label col-md-2" })
            <div class="col-md-10">
                @Html.EditorFor(Function(model) model.CategoryId, New With {.htmlAttributes = New With {.class = "form-control"}})
                @Html.ValidationMessageFor(Function(model) model.CategoryId, "", New With {.class = "text-danger"})
                @Html.ValidationMessage("EM_DuplicateCategoryId", New With {.class = "text-danger"})
            </div>
        </div>

        <div class="form-group">
            @Html.LabelFor(Function(model) model.CategoryName, htmlAttributes:= New With { .class = "control-label col-md-2" })
            <div class="col-md-10">
                @Html.EditorFor(Function(model) model.CategoryName, New With {.htmlAttributes = New With {.class = "form-control"}})
                @Html.ValidationMessageFor(Function(model) model.CategoryName, "", New With {.class = "text-danger"})
                @Html.ValidationMessage("EM_DuplicateCategoryName", New With {.class = "text-danger"})
            </div>
        </div>

        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <input type="submit" value="登録" class="btn btn-primary" />
                @Html.ActionLink("戻る", "Index", New With {.controller = "Items"}, New With {.class = "btn btn-default", .type = "button"})
            </div>
        </div>
    </div>
End Using

@Section Scripts 
    @Scripts.Render("~/bundles/jqueryval")
End Section
