/* Increment product */
$(function () {

    $("a.incproduct").click(function (e) {
        e.preventDefault();

        var productId = $(this).data("id");
        var url = "@Url.Action("IncrementProduct","Klangshop")";

        $.getJSON(url, { productId: productId }, function (data) {
            $("td.am" + productId).html(data.am);

            var price = data.am * data.price;
            var priceHtml = price.toFixed(2);

            $("td.total" + productId).html(priceHtml);

            var gt = parseFloat($("td.grandtotal span").text());
            var grandtotal = (gt + data.price).toFixed(2);

            $("td.grandtotal span").text(grandtotal);
        });
    });
});
/*-----------------------------------------------------------*/
/* Decrement product */
$(function () {

    $("a.decproduct").click(function (e) {
        e.preventDefault();

        var $this = $(this);
        var productId = $(this).data("id");
        var url = "/CartList/DecrementProduct";

        $.getJSON(url, { productId: productId }, function (data) {
            location.reload();
            if (data.am == 0) {
                $this.parent().fadeOut("fast", function () {
                    location.reload();
                });
            }
            else {
                $("td.am" + productId).html(data.qty);

                var price = data.am * data.price;
                var priceHtml = price.toFixed(2);

                $("td.total" + productId).html(priceHtml);

                var gt = parseFloat($("td.grandtotal span").text());
                var grandtotal = (gt - data.price).toFixed(2);

                $("td.grandtotal span").text(grandtotal);
            }
            location.reload();
        });
    });
});
/*-----------------------------------------------------------*/
/* Remove product */
$(function () {

    $("button.remproduct").click(function (e) {
        e.preventDefault();

        var $this = $(this);
        var productId = $(this).data("id");
        var url = "/CartList/RemoveProduct";

        $.get(url, { productId: productId }, function (data) {
            location.reload();
        });
    });
});
/*-----------------------------------------------------------*/