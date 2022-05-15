@ModelType InventoryControl.Models.Order
@Code
    ViewData("Title") = "Create"
End Code

@Styles.Render("~/Content/Orders/create.css")
<link rel="stylesheet" href="~/Content/Orders/create.css" />

<h2>発注商品一覧</h2>

@*<button class="btn btn-default" data-toggle="modal" data-target="#SearchItemModal" data-keyboard="true">商品を追加</button>*@
@*@Html.Partial("_SearchItem")*@

@Using (Html.BeginForm("Create", "Orders", Nothing, FormMethod.Post))
    @Html.AntiForgeryToken()
    @<div class="form-group text-right btn-container">
        <input type="submit" name="submit" value="商品検索" Class="btn btn-primary" />
        <input type="submit" name="submit" value="発注確定" Class="btn btn-primary" />
        <input type="submit" name="submit" value="削除（非表示）" id="delete" Class="btn btn-danger d-none" />
        @Html.ActionLink("発注キャンセル", "Cancel", New With {.controller = "Orders"}, New With {.class = "btn btn-default", .type = "button"})
    </div>
    @<div class="ordermonth-container">
        <div>
            @Html.LabelFor(Function(model) model.OrderMonth, htmlAttributes:=New With {.class = "control-label"})
        </div>
        <div class="ordermonth">
            @Html.Label("OrderMonth", DirectCast(ViewBag.displayOrderMonth, String), htmlAttributes:=New With {.class = "control-label font-weight-nm"})
        </div>
    </div>
    @<table class="table">
        <thead>
            <tr>
                <th>
                    @Html.DisplayNameFor(Function(model) model.OrderDetails(0).ItemId)
                </th>
                <th>
                    @Html.DisplayNameFor(Function(model) model.OrderDetails(0).Item.ItemName)
                </th>
                <th>
                    @Html.DisplayNameFor(Function(model) model.OrderDetails(0).Item.Category.CategoryName)
                </th>
                <th>
                    @Html.DisplayNameFor(Function(model) model.OrderDetails(0).Item.UnitPrice)
                </th>
                <th>
                    @Html.DisplayNameFor(Function(model) model.OrderDetails(0).OrderQuantity)
                </th>
                <th>
                    金額
                </th>
                <th></th>
            </tr>
        </thead>
        <tbody>
            @If Not Model.OrderDetails Is Nothing Then
                @For i As Integer = 0 To Model.OrderDetails.Count - 1
                    @*@Using (Html.BeginForm("AddItem", "Orders", Nothing, FormMethod.Post))

                        End Using*@
                    @<tr class="orderItem">
                        @Html.HiddenFor(Function(model) model.OrderId)
                        @Html.HiddenFor(Function(model) model.OrderDetails(i).OrderDetailId, New With {.class = "OrderDetailId"})
                        @Html.HiddenFor(Function(model) model.OrderDetails(i).OrderId)
                        @Html.HiddenFor(Function(model) model.OrderDetails(i).ItemId)
                        @Html.HiddenFor(Function(model) model.OrderDetails(i).Item.ItemId)
                        @Html.HiddenFor(Function(model) model.OrderDetails(i).Item.ItemName)
                        @Html.HiddenFor(Function(model) model.OrderDetails(i).Item.CategoryId)
                        @Html.HiddenFor(Function(model) model.OrderDetails(i).Item.UnitPrice)
                        <td>
                            @Html.DisplayFor(Function(model) model.OrderDetails(i).ItemId)
                        </td>
                        <td>
                            @Html.DisplayFor(Function(model) model.OrderDetails(i).Item.ItemName)
                        </td>
                        <td>
                            @Html.DisplayFor(Function(model) model.OrderDetails(i).Item.Category.CategoryName)
                        </td>
                        <td class="unitPrice">
                            @Html.DisplayFor(Function(model) model.OrderDetails(i).Item.UnitPrice, New With {.class = "unitPrice"})
                        </td>
                        <td>
                            @Html.TextBoxFor(Function(model) model.OrderDetails(i).OrderQuantity, New With {.class = "form-control tBoxWidth orderQuantity"})
                            @Html.ValidationMessageFor(Function(model) model.OrderDetails(i).OrderQuantity, "", New With {.class = "text-danger"})
                        </td>
                        <td class="price">0</td>
                        <td>
                            <a href="#" class="BtnDelete btn btn-danger" data-id="@Model.OrderDetails(i).OrderDetailId">削除</a>
                            @Html.CheckBoxFor(Function(model) model.OrderDetails(i).IsDelete, New With {.class = "IsDelete"})
                            <input type="submit" name="submit" value="削除（非表示）" id="delete" Class="btn btn-danger d-none" />
                        </td>
                    </tr>
                Next
            End If
        </tbody>
    </table>
    @<div class="justify-content-end orderAmount-container">
        <table>
            <thead>
                <tr>
                    <th>
                        @Html.LabelFor(Function(model) model.OrderAmount)
                    </th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td Class="orderAmount">
                        @Html.DisplayFor(Function(model) model.OrderAmount)
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
End Using

@Html.ValidationSummary(True, "", New With {.class = "text-danger"})

@Section Scripts
    @Scripts.Render("~/bundles/jqueryval")
    <script src="~/Scripts/Orders/create.js"></script>
End Section
