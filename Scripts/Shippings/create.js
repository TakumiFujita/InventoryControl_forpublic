$(function () {
    //ページ読み込み時、出荷明細テーブルから出荷数を取得し、金額や合計金額の計算を行う
    $(document).ready(function () {
        calculate();
    })

    //出荷数が変更された際、金額や合計金額の再計算を行う
    $('tbody tr.shippingItem').on('keyup', function () {
        calculate();
    });

    //出荷数を取得し、金額や合計金額の計算を行う
    function calculate() {
        let shippingAmountremovedComma = 0;
        let shippingAmount = 0;
        $('tr.shippingItem').each(function (i, row) {

            //単価の取得
            let unitPrice = $.trim($(row).find('.unitPrice').text());

            //カンマを取り除く関数
            function removeComma(num) {
                let removedNum = num.replace(/,/g, '');
                return parseInt(removedNum, 10);
            }

            //取得した単価からカンマを取り除く
            let unitPriceRemovedComma = removeComma(unitPrice);

            //出荷数の取得
            let quantity = $(row).find('.shippingQuantity').val();

            //単価 * 個数の計算し、変数に代入
            let priceRemovedComma = unitPriceRemovedComma * quantity;

            //カンマをつけて金額表示
            let price = priceRemovedComma.toLocaleString();
            $(row).find('.price').text(price);

            //各商品の金額を合計金額に加算していく
            shippingAmountremovedComma += priceRemovedComma;

            //カンマをつけて合計金額を表示
            shippingAmount = shippingAmountremovedComma.toLocaleString();
        });
        //合計金額の出力
        $('.shippingAmount').text(shippingAmount);
    }

    //削除ボタンを押下時、チェックボックスをONにする
    //行番号を取得し、どのデータを削除しようとしているのかを認識
    $('.BtnDelete').click(function (e) {
        let id = e.target.dataset.id;

        $('tr.shippingItem').each(function (i, row) {
            let rowid = $.trim($(row).find('.ShippingDetailId').val());
            if (id === rowid) {
                $(row).find('.IsDelete').prop('checked', true);
                $('#delete').click();
            }
        })
    })

});