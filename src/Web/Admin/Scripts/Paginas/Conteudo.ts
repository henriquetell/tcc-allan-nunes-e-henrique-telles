class Conteudo {
  private static ins: Conteudo;
  static get instance() {
    return this.ins || (this.ins = new this());
  }
  init(ev: any): void {
    const that: Conteudo = this;

    if ($("#UploadDropzone").length > 0) {
      const myDropzone = new Dropzone("#UploadDropzone",
        {
          paramName: "Arquivo",
          maxFilesize: 10,
          autoProcessQueue: true,
          parallelUploads: 1,
          maxFiles: 7,
          uploadMultiple: false,
          dictDefaultMessage: "Arraste ou clique aqui para adicionar anexos.",
          dictFallbackMessage: "Seu navegador não suporta arrastar e soltar para upload de arquivos.",
          dictFallbackText: "Por favor, use o formulário de fallback abaixo para fazer upload de seus arquivos.",
          dictFileTooBig: "O arquivo é muito grande ({{filesize}}MiB). Tamanho máximo do arquivo: {{maxFilesize}}MiB.",
          dictInvalidFileType: "Você não pode enviar arquivos desse tipo.",
          dictResponseError: "O servidor respondeu o código {{statusCode}}.",
          dictCancelUpload: "Cancelar upload",
          dictCancelUploadConfirmation: "Tem certeza que deseja cancelar esse upload?",
          dictRemoveFileConfirmation: null,
          dictMaxFilesExceeded: "Você atingiu a quantidade máxima de envio de arquivos.",
          acceptedFiles: ".png,.jpg,.jpeg,.gif,.pdf",
          init: function () {
            var dz = this;

            this.on("error", (file: any) => {
              if (!file.accepted) {
                toastr["error"](`Não é possível enviar o arquivo: ${file.name}`);
              }
            });
            this.on("complete", (file: any) => {
              if (file.status !== "error") {
                var msg = file.xhr.responseText.replace(/[\\"]/g, "");
                var tipo = file.xhr.status === 200 ? "success" : "error";
                toastr[tipo](file.name + " - " + msg);
              }
              if (dz.files.length <= 0)
                window.location.reload();
            });
            this.on("success", (file: any) => {
              dz.removeFile(file);
            });
          }
        });
    }

    if ($("select#IdConteudoChave").length > 0) {
      $("select#IdConteudoChave").on("change", function (event: JQuery.Event) {
        that.conteudoChaveOnChange(event, $(this));
      }).trigger("change");
    }
  }

  conteudoChaveOnChange(event: JQuery.Event, sender: JQuery<Element>): void {
    const value = sender.val() as string;
    $("pre").each((index: any, element: any) => {
      if ($(element).data("conteudochave") === parseInt(value)) {
        $(element).parent().parent().show();
      } else {
        $(element).parent().parent().hide();
      }
    });
  }
}

Admin.instance.paginas["Conteudo"] = Conteudo.instance;