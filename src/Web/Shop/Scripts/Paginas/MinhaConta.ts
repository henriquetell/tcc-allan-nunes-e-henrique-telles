class MinhaConta {
    private static ins: MinhaConta;
    static get instance() {
        return this.ins || (this.ins = new this());
    }
    init(ev: any): void {
        const self = this;

        $("#AlterarSenha").on("change", function () {
            if ($(this).is(":checked")) {
                $("[alterar-senha]").show();
            } else {
                $("[alterar-senha]").hide();
            }
        }).trigger("change");

    }
}
Site.instance.paginas["MinhaConta"] = MinhaConta.instance;