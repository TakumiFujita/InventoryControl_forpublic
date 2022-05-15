$(function () {
    //ページ読み込み時、返品明細テーブルから返品数を取得し、金額や合計金額の計算を行う
    $(document).ready(function () {
        calculate();
    })

    //返品数が変更された際、金額や合計金額の再計算を行う
    $('tbody tr.returnItem').on('keyup', function () {
        calculate();
    });

    //返品数を取得し、金額や合計金額の計算を行う
    function calculate() {
        let returnAmountremovedComma = 0;
        let returnAmount = 0;
        $('tr.returnItem').each(function (i, row) {

            //単価の取得
            let unitPrice = $.trim($(row).find('.unitPrice').text());

            //カンマを取り除く関数
            function removeComma(num) {
                let removedNum = num.replace(/,/g, '');
                return parseInt(removedNum, 10);
            }

            //取得した単価からカンマを取り除く
            let unitPriceRemovedComma = removeComma(unitPrice);

            //返品数の取得
            let quantity = $(row).find('.returnQuantity').val();

            //単価 * 個数の計算し、変数に代入
            let priceRemovedComma = unitPriceRemovedComma * quantity;

            //カンマをつけて金額表示
            let price = priceRemovedComma.toLocaleString();
            $(row).find('.price').text(price);

            //各商品の金額を合計金額に加算していく
            returnAmountremovedComma += priceRemovedComma;

            //カンマをつけて合計金額を表示
            returnAmount = returnAmountremovedComma.toLocaleString();
        });
        //合計金額の出力
        $('.returnAmount').text(returnAmount);
    }

    //削除ボタンを押下時、チェックボックスをONにする
    //行番号を取得し、どのデータを削除しようとしているのかを認識
    $('.BtnDelete').click(function (e) {
        let id = e.target.dataset.id;

        $('tr.returnItem').each(function (i, row) {
            let rowid = $.trim($(row).find('.ReturnDetailId').val());
            if (id === rowid) {
                $(row).find('.IsDelete').prop('checked', true);
                $('#delete').click();
            }
        })
    })

});