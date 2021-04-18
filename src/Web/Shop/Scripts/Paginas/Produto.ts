class Produto {
    private static ins: Produto;
    static get instance() {
        return this.ins || (this.ins = new this());
    }
    init(ev: any): void {
        const self = this;

        $("form#produto").on("submit", function (event) {
            const val = $('[name="IdProdutoSku"]:checked').val();
            return val > 0;
        });

    }
}
Site.instance.paginas["Produto"] = Produto.instance;