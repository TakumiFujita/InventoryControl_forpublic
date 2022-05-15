@ModelType InventoryControl.Models.Shipping
@Code
    ViewData("Title") = "Create"
End Code

@Styles.Render("~/Content/Shippings/create.css")
<link rel="stylesheet" href="~/Content/Shippings/create.css" />

<h2>出荷商品一覧</h2>

@Using (Html.BeginForm("Create", "Shippings", Nothing, FormMethod.Post))
    @Html.AntiForgeryToken()
    @<div class="form-group text-right btn-container">
        <input type="submit" name="submit" value="商品検索" Class="btn btn-primary" />
        <input type="submit" name="submit" value="出荷確定" Class="btn btn-primary" />
        <input type="submit" name="submit" value="削除（非表示）" id="delete" Class="btn btn-danger d-none" />
        @Html.ActionLink("出荷キャンセル", "Cancel", New With {.controller = "Shippings"}, New With {.class = "btn btn-default", .type = "button"})
    </div>
    @<div class="shippingmonth-container">
        <div>
            @Html.LabelFor(Function(model) model.ShippingMonth, htmlAttributes:=New With {.class = "control-label"})
        </div>
        <div class="shippingmonth">
            @Html.Label("ShippingMonth", DirectCast(ViewBag.displayShippingMonth, String), htmlAttributes:=New With {.class = "control-label font-weight-nm"})
        </div>
    </div>
    @<table class="table">
        <thead>
            <tr>
                <th>
                    @Html.DisplayNameFor(Function(model) model.ShippingDetails(0).ItemId)
                </th>
                <th>
                    @Html.DisplayNameFor(Function(model) model.ShippingDetails(0).Item.ItemName)
                </th>
                <th>
                    @Html.DisplayNameFor(Function(model) model.ShippingDetails(0).Item.Category.CategoryName)
                </th>
                <th>
                    @Html.DisplayNameFor(Function(model) model.ShippingDetails(0).Item.UnitPrice)
                </th>
                <th>
                    @Html.DisplayNameFor(Function(model) model.ShippingDetails(0).ShippingQuantity)
                </th>
                <th>
                    金額
                </th>
                <th></th>
            </tr>
        </thead>
        <tbody>
            @If Not Model.ShippingDetails Is Nothing Then
                @For i As Integer = 0 To Model.ShippingDetails.Count - 1
                    @<tr class="shippingItem">
                        @Html.HiddenFor(Function(model) model.ShippingId)
                        @Html.HiddenFor(Function(model) model.ShippingDetails(i).ShippingDetailId, New With {.class = "ShippingDetailId"})
                        @Html.HiddenFor(Function(model) model.ShippingDetails(i).ShippingId)
                        @Html.HiddenFor(Function(model) model.ShippingDetails(i).ItemId)
                        @Html.HiddenFor(Function(model) model.ShippingDetails(i).Item.ItemId)
                        @Html.HiddenFor(Function(model) model.ShippingDetails(i).Item.ItemName)
                        @Html.HiddenFor(Function(model) model.ShippingDetails(i).Item.CategoryId)
                        @Html.HiddenFor(Function(model) model.ShippingDetails(i).Item.UnitPrice)
                        <td>
                            @Html.DisplayFor(Function(model) model.ShippingDetails(i).ItemId)
                        </td>
                        <td>
                            @Html.DisplayFor(Function(model) model.ShippingDetails(i).Item.ItemName)
                        </td>
                        <td>
                            @Html.DisplayFor(Function(model) model.ShippingDetails(i).Item.Category.CategoryName)
                        </td>
                        <td class="unitPrice">
                            @Html.DisplayFor(Function(model) model.ShippingDetails(i).Item.UnitPrice, New With {.class = "unitPrice"})
                        </td>
                        <td>
                            @Html.TextBoxFor(Function(model) model.ShippingDetails(i).ShippingQuantity, New With {.class = "form-control tBoxWidth shippingQuantity"})
                            @Html.ValidationMessageFor(Function(model) model.ShippingDetails(i).ShippingQuantity, "", New With {.class = "text-danger"})
                        </td>
                        <td class="price">0</td>
                        <td>
                            <a href="#" class="BtnDelete btn btn-danger" data-id="@Model.ShippingDetails(i).ShippingDetailId">削除</a>
                            @Html.CheckBoxFor(Function(model) model.ShippingDetails(i).IsDelete, New With {.class = "IsDelete"})
                            <input type="submit" name="submit" value="削除（非表示）" id="delete" Class="btn btn-danger d-none" />
                        </td>
                    </tr>
                Next
            End If
        </tbody>
    </table>
    @<div class="justify-content-end shippingAmount-container">
        <table>
            <thead>
                <tr>
                    <th>
                        @Html.LabelFor(Function(model) model.ShippingAmount)
                    </th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td Class="shippingAmount">
                        @Html.DisplayFor(Function(model) model.ShippingAmount)
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
End Using

@Html.ValidationSummary(True, "", New With {.class = "text-danger"})

@Section Scripts
    @Scripts.Render("~/bundles/jqueryval")
    <script src="~/Scripts/Shippings/create.js"></script>
End Section