class Produto {
    private static ins: Produto;
    static get instance() {
        return this.ins || (this.ins = new this());
    }
    init(ev: any): void {
        if ($("#UploadDropzone").length > 0) {
            const myDropzone = new Dropzone("#UploadDropzone", {
                maxFilesize: 2,
                autoProcessQueue: true,
                parallelUploads: 1,
                maxFiles: 20,
                uploadMultiple: false,
                dictDefaultMessage: "Arraste ou clique aqui para adicionar imagens.",
                dictFallbackMessage: "Seu navegador não suporta arrastar e soltar para envio de arquivos.",
                dictFallbackText: "Por favor, use o formulário de abaixo para efetuar o envio de seus arquivos.",
                dictFileTooBig: "O arquivo é muito grande ({{filesize}}MiB). Tamanho máximo do arquivo: {{maxFilesize}}MiB.",
                dictInvalidFileType: "Você não pode enviar arquivos desse tipo.",
                dictResponseError: "O servidor respondeu o código {{statusCode}}.",
                dictCancelUpload: "Cancelar envio",
                dictCancelUploadConfirmation: "Tem certeza que deseja cancelar esse envio?",
                dictRemoveFileConfirmation: null,
                dictMaxFilesExceeded: "Você atingiu a quantidade máxima de envio de arquivos.",
                paramName: "Arquivo",
                acceptedFiles: "image/*",
                init: function () {
                    var dz = this;
                    this.on("error", (file: any) => {
                        if (file.accepted) {
                            toastr["error"]("Não é possível enviar o arquivo: " + file.name);
                        }
                    });
                    this.on("complete", (file: any) => {
                        if (file.status !== "error") {
                            var msg = file.xhr.responseText.replace(/[\\"]/g, "");
                            var tipo = file.xhr.status === 200 ? "success" : "error";
                            toastr[tipo](file.name + " - " + msg);
                            if (dz.files.length <= 0) {
                                setTimeout(() => {
                                    window.location.reload();
                                }, 3000);
                            }
                        }
                    });
                    this.on("success", (file: any) => {
                        dz.removeFile(file);
                    });
                }
            });
        }


        if ($("#FormOrdernarImagens").length > 0) {
            $("#row-imagens").sortable({
                handle: ".ibox-content",
                connectWith: "#row-imagens",
                tolerance: "pointer",
                forcePlaceholderSize: true,
                opacity: 0.8,
                update: (event: any, ui: any) => {
                    var html = "";
                    $("#row-imagens [data-id]").each((index: any, item: any) => {
                        html += `<input type="hidden" name="imagem" value="${$(item).data("id")}" />`;
                    });
                    $("#FormOrdernarImagens").append(html);
                    $("#FormOrdernarImagens").submit();
                }
            }).disableSelection();
        }

        if ($("input[name=\"Quantidade\"]").length > 0) {
            $("input[name=\"Quantidade\"]").each((index, item) => {
                var id = $(item).attr("id");
                $(`#${id}`).btnGroupSinal();
            })
        }

        if ($("input[movimentar]").length > 0) {
            $("input[movimentar]").on("ifChanged", function () {
                var id = $(this).attr("movimentar");
                if ($(this).is(":checked")) {
                    $(`div[movimentar="${id}"]`).fadeIn();
                } else {
                    $(`div[movimentar="${id}"]`).fadeOut();
                }

            }).trigger("ifChanged");
        }

        if ($("div[novo-sku]").length > 0) {
            $("div[novo-sku]").hide();
            $("button#btnNovo").on("click", function () {
                $(this).closest(".row").hide();
                $("div[novo-sku]").fadeIn();
            });
        }
    }
}

Admin.instance.paginas["Produto"] = Produto.instance;