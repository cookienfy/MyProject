(function ($) {

    $.fn.extend({
        getAlert: function (aa) {
            $(this).click(function () {
                alert(aa);
            });
        }
    });

})(jQuery);