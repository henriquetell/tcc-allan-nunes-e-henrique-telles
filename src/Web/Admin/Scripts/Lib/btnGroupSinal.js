(function ($) {
    "use strict";
    $.fn.btnGroupSinal = function () {
        var f = {
            init: function () {
                $(f.options.seletor).on("change", f.input_onChange).trigger("change");
                var btn = $("[button-for=\"" + f.options.seletor + "\"]");
                btn.unbind("click");
                btn.on("click", f.btn_onClick);
            },
            input_onChange: function () {
                f.alterar($("[button-for=\"" + f.options.seletor + "\"]"), $(this), false);
            },
            btn_onClick: function () {
                f.alterar($(this), $(f.options.seletor), true);
            },
            alterar: function (btn, input, mudarSinal) {
                try {
                    var val = 0;
                    var v = parseInt(input.val());
                    if (v > 0 || v < 0) {
                        val = v;
                        if (mudarSinal) {
                            val = val * -1;
                            input.val(val);
                        }
                    }
                    if (val >= 0) {
                        if (isNaN(v) || val === 0) {
                            input.val("");
                        }
                        $("i", btn).toggleClass("fa fa-minus", false).toggleClass("fa fa-plus", true);
                        $(btn).toggleClass("btn btn-warning", false).toggleClass("btn btn-primary", true);
                    } else {
                        $("i", btn).toggleClass("fa fa-plus", false).toggleClass("fa fa-minus", true);
                        $(btn).toggleClass("btn btn-primary", false).toggleClass("btn btn-warning", true);
                    }
                } catch (e) {
                    console.log("Erro ao converter o valor!");
                }
            },
            options: {
                seletor: ""
            }
        };
        f.options.seletor = "#" + $(this).attr("id");

        $(function () {
            f.init();
        });
    };
})(jQuery);