declare function swal(opt: any): any;


class Admin {
    paginas: any = {};
    layout: any = {};
    private static ins: Admin;
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

        if (Dropzone) {
            Dropzone.autoDiscover = false;
        }

        if (toastr) {
            toastr.options.closeButton = true;
            toastr.options.debug = false;
            toastr.options.progressBar = true;
            toastr.options.preventDuplicates = true;
            toastr.options.positionClass = "toast-top-right";
            toastr.options.showDuration = 400;
            toastr.options.hideDuration = 1000;
            toastr.options.timeOut = 10000;
            toastr.options.extendedTimeOut = 5000;
            toastr.options.showEasing = "swing";
            toastr.options.hideEasing = "linear";
            toastr.options.showMethod = "fadeIn";
            toastr.options.hideMethod = "fadeOut";
        }

        Admin.bindTitle();
        Admin.bindPopover();

        $(".i-checks").iCheck({
            checkboxClass: "icheckbox_square-green",
            radioClass: "iradio_square-green"
        });

        $("[datepicker]").datepicker({
            format: "dd/mm/yyyy",
            autoclose: true,
            language: "pt-BR"
        });

        $("[summernote]").each((index: number, item: any) => {
            var height = $(item).data("height");
            if (height === undefined || height === "")
                height = 500;
            $(item).summernote({
                height: height,
                lang: "pt-BR",
                placeholder: $(item).attr("placeholder"),
                toolbar: [
                    ['style', ['bold', 'italic', 'underline', 'clear']],
                    ['fontsize', ['fontsize']],
                    ["font", ['']],
                    ['para', ['ul', 'ol', 'paragraph']],
                    ['height', ['height']],
                    ['insert', ['table', 'hr']],
                    ['Misc', ['codeview']]
                ]
            });
        });

        $("[chosen]").chosen({ width: "100%" });

        $("#inputImage").change(function () {
            const fileReader = new FileReader();
            const files = (<HTMLInputElement>this).files;
            if (!files.length) {
                return;
            }
            const file = files[0];
            if (/^image\/\w+$/.test(file.type)) {
                fileReader.readAsDataURL(file);
                fileReader.onload = function () {
                    $("#inputImage").val("");
                    $("#imagemCropper").cropper("reset", true).cropper("replace", this.result);
                };
            } else {
                toastr["error"]("Por favor, seleciona apenas imagens!", "Apenas Imagens");
            }
        });

        $("#SalvarImagem").click(function () {
            var imagemUsuario = $("#imagemCropper").cropper("getCroppedCanvas", { width: 150, height: 150 }).toDataURL();

            $.ajax($(this).data("url"), {
                method: "POST",
                data: { imagem: imagemUsuario },
                success: (dados) => {
                    if (dados) {
                        $("#alterarImagemModal").modal("hide");
                        $(".img-user").attr("src", imagemUsuario);
                        toastr['success']("Imagem alterada com sucesso!");
                    }
                },
                error: () => {
                    toastr['error']("Não foi possível alterar sua imagem, tente novamente!");
                }
            });
        });

        $("#uploadImg").click(() => {
            window.open($("#imagemCropper").cropper("getDataURL"));
        });

        $("#zoomIn").click(() => {
            $("#imagemCropper").cropper("zoom", 0.1);
        });

        $("#zoomOut").click(() => {
            $("#imagemCropper").cropper("zoom", -0.1);
        });


        $((e: any) => {
            for (let prop in Admin.instance.layout) {
                if (Admin.instance.layout.hasOwnProperty(prop)) {
                    const cia = Admin.instance.layout[prop];
                    if (cia && cia.init && $.isFunction(cia.init))
                        cia.init(e);
                }
            }

            Admin.bindPagina($("[data-pagina]"), e);
        });

        $((document) as any).on("submit", "form[data-form-excluir]", function (e) {
            var form = this;
            if (!$(form).data("form-excluir")) {
                e.preventDefault();
                swal({
                    title: "Você tem certeza?",
                    text: "Não é possível reverter essa operação!",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#ED5565",
                    cancelButtonColor: "#c2c2c2",
                    confirmButtonText: "Sim",
                    cancelButtonText: "Cancelar"
                }).then((result: any) => {
                    if (result.value) {
                        $(form).data("form-excluir", true);
                        $(form).submit();
                    }
                });
                return false;
            }
            return true;
        });

        $((document) as any).on("click", "button[chosen-todos]", function (e) {
            $($(this).data("target") + " option:not([selected])").attr("selected", "selected");
            $($(this).data("target")).trigger("change").trigger("chosen:updated");
        });

        $.extend(true, $.magnificPopup.defaults, {
            tClose: "Fechar (Esc)",
            tLoading: "Carregando...",
            gallery: {
                tPrev: "Anterior (Tecla seta esquerda)",
                tNext: "Próxima (Tecla seta direita)",
                tCounter: "%curr% de %total%"
            },
            image: {
                tError: 'Não foi possível carregar <a href="%url%">a imagem</a>.'
            },
            ajax: {
                tError: 'Não foi possível carregar o <a href="%url%">conteúdo</a>.'
            }
        });
    }

    static bindTitle(target?: any): void {
        var itens = $("[title]:not([data-toggle=\"popover\"])");
        if (target !== undefined)
            itens = $("[title]:not([data-toggle=\"popover\"])", target);
        itens.tooltip();
    }

    static bindPopover(target?: any): void {
        var itens = $('[data-toggle="popover"]');
        if (target !== undefined)
            itens = $('[data-toggle="popover"]', target);
        itens.each((i, e) => {
            let options = {
                html: $(e).is("[data-toggle-html='true']")
            };
            $(e).popover(options);
        });
    }

    static bindPagina(target: any, e?: any): void {
        target.each((index: number, item: any) => {
            if (!$(item).data("pagina-carregado")) {
                $(item).data("pagina-carregado", true);
                const cia = Admin.instance.paginas[$(item).data("pagina")];
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