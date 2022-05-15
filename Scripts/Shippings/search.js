$(function () {
    //出荷一覧画面に商品が登録が存在する状態で「在庫一覧画面」へ戻ろうとすると、確認ダイアログを表示
    $('#deleteConfirm').click(function () {
        if (confirm('出荷商品一覧に追加した商品は全て削除されます。\nよろしいでしょうか？')) {
            return true;
        } else {
            return false;
        }
    })
});