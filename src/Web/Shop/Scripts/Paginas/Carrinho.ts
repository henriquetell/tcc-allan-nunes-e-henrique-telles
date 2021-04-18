class Carrinho {
    private static ins: Carrinho;
    static get instance() {
        return this.ins || (this.ins = new this());
    }
    init(ev: any): void {
        const self = this;

        $(".esh-basket-minus").on("click", function (event: JQuery.Event) {
            event.preventDefault();
            self.post($(this));
        });

        $(".esh-basket-plus").on("click", function (event: JQuery.Event) {
            event.preventDefault();
            self.post($(this));
        });

        $(".esh-basket-remove").on("click", function (event: JQuery.Event) {
            event.preventDefault();
            self.post($(this));
        });

        $(".esh-basket-items").each((index, item) => {
            var value = parseInt($(".esh-basket-input").val() as string);
            if (value > 1) {
                $(".esh-basket-minus", item).show();
            } else {
                $(".esh-basket-minus", item).hide();
            }
        });

    }

    post(sender: JQuery<HTMLElement>): void {
        let url = sender.attr("href");
        if (url) {
            let data = {
                "idProdutoSku": sender.data("idprodutosku"),
                "idCarrinho": sender.data("idcarrinho"),
                "idCarrinhoItem": sender.data("idcarrinhoitem"),
            };

            $.ajax({
                ajaxLoader: false,
                cache: false,
                method: "POST",
                data: data,
                async: true,
                url: url,
                beforeSend: () => {
                    sender.prop("disabled", true);
                },
                success: () => {
                    window.location.reload();
                },
                error: (err: any) => {
                    alert(err.responseText);
                },
                complete: () => {
                    sender.prop("disabled", false);
                }
            });
        }
    }
}
Site.instance.paginas["Carrinho"] = Carrinho.instance;