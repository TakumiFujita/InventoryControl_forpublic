$(function () {
    //ページ読み込み時、発注明細テーブルから発注数を取得し、金額や合計金額の計算を行う
    $(document).ready(function () {
        calculate();
    })

    //発注数が変更された際、金額や合計金額の再計算を行う
    $('tbody tr.orderItem').on('keyup', function () {
        calculate();
    });

    //発注数を取得し、金額や合計金額の計算を行う
    function calculate() {
        let orderAmountremovedComma = 0;
        let orderAmount = 0;
        $('tr.orderItem').each(function (i, row) {

            //単価の取得
            let unitPrice = $.trim($(row).find('.unitPrice').text());

            //カンマを取り除く関数
            function removeComma(num) {
                let removedNum = num.replace(/,/g, '');
                return parseInt(removedNum, 10);
            }

            //取得した単価からカンマを取り除く
            let unitPriceRemovedComma = removeComma(unitPrice);

            //発注数の取得
            let quantity = $(row).find('.orderQuantity').val();

            //単価 * 個数の計算し、変数に代入
            let priceRemovedComma = unitPriceRemovedComma * quantity;

            //カンマをつけて金額表示
            let price = priceRemovedComma.toLocaleString();
            $(row).find('.price').text(price);

            //各商品の金額を合計金額に加算していく
            orderAmountremovedComma += priceRemovedComma;

            //カンマをつけて合計金額を表示
            orderAmount = orderAmountremovedComma.toLocaleString();
        });
        //合計金額の出力
        $('.orderAmount').text(orderAmount);
    }

    //削除ボタンを押下時、チェックボックスをONにする
    //行番号を取得し、どのデータを削除しようとしているのかを認識
    $('.BtnDelete').click(function (e) {
        let id = e.target.dataset.id;

        $('tr.orderItem').each(function (i, row) {
            let rowid = $.trim($(row).find('.OrderDetailId').val());
            if (id === rowid) {
                $(row).find('.IsDelete').prop('checked', true);
                $('#delete').click();
            }
        })
    })

    //削除ボタンを押下時、チェックボックスをONにする
    //行番号を取得し、どのデータを削除しようとしているのかを認識
    //$('.BtnDelete').click(function (e) {
    //    let id = e.target.dataset.id;

    //    $('tr.orderItem').each(function (i, row) {
    //        let rowid = $.trim($(row).find('.OrderDetailId').val());
    //        if (id === rowid) {
    //            $(row).find('.IsDelete').prop('checked', true);
    //            //$('#test2').submit();
    //            //$('form').submit();
    //            //$('#delete').submit();
    //            //document.test.submit();
    //        }
    //    })
    //})
});

//単価の取得
//個数の取得
//単価 * 個数の計算し、変数に代入
//その変数を合計の変数に代入（+=変数）
//一行一行に対して以上を行う

//body > div.container.body - content > table > tbody: nth - child(2) > tr > td.orderQuantity > input

//document.querySelector("body > div.container.body-content > table > tbody:nth-child(2) > tr > td.orderQuantity > input")