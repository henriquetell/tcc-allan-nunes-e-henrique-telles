class Site {
    paginas: any = {};
    layout: any = {};
    private static ins: Site;
    static get instance() {
        return this.ins || (this.ins = new this());
    }

    constructor() { }

    onLoad: any = [];

    init(): void {

        $((document) as any).siteMask();

        if ($.validator) {
            $.validator.methods['range'] = function (value: any, element: any, param: any) {
                const globalizedValue = value.replace(",", ".");
                return this.optional(element) || (globalizedValue >= param[0] && globalizedValue <= param[1]);
            };
            $.validator.methods['number'] = function (value: any, element: any) {
                return this.optional(element) || /^-?(?:\d+|\d{1,3}(?:[\s\.,]\d{3})+)(?:[\.,]\d+)?$/.test(value);
            };
        }

        $((e: any) => {
            for (let prop in Site.instance.layout) {
                if (Site.instance.layout.hasOwnProperty(prop)) {
                    const cia = Site.instance.layout[prop];
                    if (cia && cia.init && $.isFunction(cia.init))
                        cia.init(e);
                }
            }

            Site.bindPagina($("[data-pagina]"), e);
        });

    }

    static bindPagina(target: any, e?: any): void {
        target.each((index: number, item: any) => {
            if (!$(item).data("pagina-carregado")) {
                $(item).data("pagina-carregado", true);
                const cia = Site.instance.paginas[$(item).data("pagina")];
                if (cia && cia.init && $.isFunction(cia.init))
                    cia.init(e, $(item).data("init"));
            }
        });
    }

    static bindSelect(event: JQuery.Event, url: string, sender: any, target: any, data?: any): void {
        if (!url || !sender || !target)
            return;

        $.ajax({
            ajaxLoader: false,
            cache: false,
            method: "POST",
            data: data,
            async: true,
            url: url,
            success: (response: any) => {
                var html: string = "";
                if ($(target).data("selecione") ||
                    $(target).data("selecione") === null ||
                    $(target).data("selecione") === undefined)
                    html = "<option value=\"\">Selecione...</option>";

                if (response != null && response !== undefined && response.length > 0) {
                    $(target).prop("disabled", false);
                    var grupos: any = [];
                    $(response).each((index: number, item: any) => {
                        if (item.group && item.group.name && grupos.indexOf(item.group.name) === -1) {
                            grupos.push(item.group.name);
                        }
                    });
                    if (grupos.length === 0) {
                        $(response).each((index: number, item: any) => {
                            var selecionado = $(target).data("value") == item.value ||
                                ($(target).data("value") &&
                                    $(target).data("value").indexOf(parseInt(item.value)) !== -1)
                                ? "selected=\"selected\""
                                : "";
                            html += `<option value="${item.value}"${selecionado}>${item.text}</option>`;
                        });
                    } else {
                        $(grupos).each((g: number, grupo: any) => {
                            html += `<optgroup label="${grupo}">`;
                            $(response).each((index: number, item: any) => {
                                if (item.group && item.group.name && item.group.name === grupo) {
                                    const selecionado = $(target).data("value") == item.value ||
                                        ($(target).data("value") &&
                                            $(target).data("value").indexOf(parseInt(item.value)) !== -1)
                                        ? "selected=\"selected\""
                                        : "";
                                    html += `<option value="${item.value}"${selecionado}>${item.text}</option>`;
                                }
                            });
                            html += "</optgroup>";
                        });
                    }
                } else if ($(target).attr("chosen") !== undefined) {
                    $(target).prop("disabled", $(target).data("disabled"));
                }
                $(target).html(html);
                if ($(target).attr("chosen") !== undefined) {
                    $(target).trigger("chosen:updated");
                }
                $(target).trigger("change");
            }
        });
    }
}