@ModelType InventoryControl.Models.ReturnItem
@Code
    ViewData("Title") = "Create"
End Code

@Styles.Render("~/Content/Returns/create.css")
<link rel="stylesheet" href="~/Content/Returns/create.css" />

<h2>返品商品一覧</h2>

@Using (Html.BeginForm("Create", "Returns", Nothing, FormMethod.Post))
    @Html.AntiForgeryToken()
    @<div class="form-group text-right btn-container">
        <input type="submit" name="submit" value="商品検索" Class="btn btn-primary" />
        <input type="submit" name="submit" value="返品確定" Class="btn btn-primary" />
        <input type="submit" name="submit" value="削除（非表示）" id="delete" Class="btn btn-danger d-none" />
        @Html.ActionLink("返品キャンセル", "Cancel", New With {.controller = "Returns"}, New With {.class = "btn btn-default", .type = "button"})
    </div>
    @<div class="returnmonth-container">
        <div>
            @Html.LabelFor(Function(model) model.ReturnMonth, htmlAttributes:=New With {.class = "control-label"})
        </div>
        <div class="returnmonth">
            @Html.Label("ReturnMonth", DirectCast(ViewBag.displayReturnMonth, String), htmlAttributes:=New With {.class = "control-label font-weight-nm"})
        </div>
    </div>
    @<table class="table">
        <thead>
            <tr>
                <th>
                    @Html.DisplayNameFor(Function(model) model.ReturnDetails(0).ItemId)
                </th>
                <th>
                    @Html.DisplayNameFor(Function(model) model.ReturnDetails(0).Item.ItemName)
                </th>
                <th>
                    @Html.DisplayNameFor(Function(model) model.ReturnDetails(0).Item.Category.CategoryName)
                </th>
                <th>
                    @Html.DisplayNameFor(Function(model) model.ReturnDetails(0).Item.UnitPrice)
                </th>
                <th>
                    @Html.DisplayNameFor(Function(model) model.ReturnDetails(0).ReturnQuantity)
                </th>
                <th>
                    金額
                </th>
                <th></th>
            </tr>
        </thead>
        <tbody>
            @If Not Model.ReturnDetails Is Nothing Then
                @For i As Integer = 0 To Model.ReturnDetails.Count - 1
                    @<tr class="returnItem">
                        @Html.HiddenFor(Function(model) model.ReturnId)
                        @Html.HiddenFor(Function(model) model.ReturnDetails(i).ReturnDetailId, New With {.class = "ReturnDetailId"})
                        @Html.HiddenFor(Function(model) model.ReturnDetails(i).ReturnId)
                        @Html.HiddenFor(Function(model) model.ReturnDetails(i).ItemId)
                        @Html.HiddenFor(Function(model) model.ReturnDetails(i).Item.ItemId)
                        @Html.HiddenFor(Function(model) model.ReturnDetails(i).Item.ItemName)
                        @Html.HiddenFor(Function(model) model.ReturnDetails(i).Item.CategoryId)
                        @Html.HiddenFor(Function(model) model.ReturnDetails(i).Item.UnitPrice)
                        <td>
                            @Html.DisplayFor(Function(model) model.ReturnDetails(i).ItemId)
                        </td>
                        <td>
                            @Html.DisplayFor(Function(model) model.ReturnDetails(i).Item.ItemName)
                        </td>
                        <td>
                            @Html.DisplayFor(Function(model) model.ReturnDetails(i).Item.Category.CategoryName)
                        </td>
                        <td class="unitPrice">
                            @Html.DisplayFor(Function(model) model.ReturnDetails(i).Item.UnitPrice, New With {.class = "unitPrice"})
                        </td>
                        <td>
                            @Html.TextBoxFor(Function(model) model.ReturnDetails(i).ReturnQuantity, New With {.class = "form-control tBoxWidth returnQuantity"})
                            @Html.ValidationMessageFor(Function(model) model.ReturnDetails(i).ReturnQuantity, "", New With {.class = "text-danger"})
                        </td>
                        <td class="price">0</td>
                        <td>
                            <a href="#" class="BtnDelete btn btn-danger" data-id="@Model.ReturnDetails(i).ReturnDetailId">削除</a>
                            @Html.CheckBoxFor(Function(model) model.ReturnDetails(i).IsDelete, New With {.class = "IsDelete"})
                            <input type="submit" name="submit" value="削除（非表示）" id="delete" Class="btn btn-danger d-none" />
                        </td>
                    </tr>
                Next
            End If
        </tbody>
    </table>
    @<div class="justify-content-end returnAmount-container">
        <table>
            <thead>
                <tr>
                    <th>
                        @Html.LabelFor(Function(model) model.ReturnAmount)
                    </th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td Class="returnAmount">
                        @Html.DisplayFor(Function(model) model.ReturnAmount)
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
End Using

@Html.ValidationSummary(True, "", New With {.class = "text-danger"})

@Section Scripts
    @Scripts.Render("~/bundles/jqueryval")
    <script src="~/Scripts/Returns/create.js"></script>
End Section