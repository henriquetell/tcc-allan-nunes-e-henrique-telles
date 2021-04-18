class Pjax {
    private static ins: Pjax;
    constructor() { }
    static get instance() {
        return this.ins || (this.ins = new this());
    }
    init(e: any): void {
        const that: Pjax = this;
        $((document) as any).on("submit", "form[data-pjax]", function (event: JQuery.Event) {
            event.preventDefault();
            if ($(this).valid()) {
                $.pjax.submit(event, that.pjaxOptions($(this)));
            } else {
                toastr["error"]("Por favor, preencha o formulário corretamente!");
                return false;
            }
        });

        $((document) as any).on("click", "a[data-pjax]", function (event: JQuery.Event) {
            $.pjax.click(event, that.pjaxOptions($(this)));
        });

        $((document) as any).on("pjax:timeout", (event: JQuery.Event) => {
            event.preventDefault();
        });

        $((document) as any).on("pjax:success", (data, status, xhr, options) => {
            that.validateForm(data);
            that.showModal(data);
            Admin.bindPagina($("[data-pagina]", data.target) as any);
            Admin.bindTitle(data.target);
            Admin.bindPopover(data.target);
        });
    }
    pjaxOptions(form: any): any {
        const opts: any = {
            timeout: 5000,
            push: false,
            replace: false,
            scrollTo: false,
            maxCacheLength: 20,
            type: "GET",
            dataType: "html",
            container: "body"
        };
        for (let prop in opts) {
            if (opts.hasOwnProperty(prop)) {
                let value = $(form).data(`pjax-${prop}`);
                if (prop === "type" && !value) {
                    value = $(form).attr("method");
                }
                if (value) {
                    opts[prop] = value;
                }
            }
        }
        return opts;
    }
    validateForm(data: any): void {
        $(data.target).removeData("validator");
        $(data.target).removeData("unobtrusiveValidation");
        $.validator.unobtrusive.parse(data.target);
        $(data.target).siteMask();
    }
    showModal(data: any): void {
        var md = $('.modal', data.target);
        if (md.length > 0) {
            md.modal('toggle');
            md.modal('show');
        }

    }
}
Admin.instance.layout["Pjax"] = Pjax.instance;