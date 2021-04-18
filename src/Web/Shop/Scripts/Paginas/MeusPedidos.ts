class MeusPedidos {
    private static ins: MeusPedidos;
    static get instance() {
        return this.ins || (this.ins = new this());
    }
    init(ev: any): void {
        const self = this;
        $('#meus-pedidos').collapse({
            toggle: false
        });

        $('#meus-pedidos a.esh-orders-link').on('click', function (event) {
            const action = $(this).data('action');
            const target = $(this).data('target');
            $.ajax({
                async: false,
                method: 'GET',
                url: action,
                success: function (response: any) {
                    $(target).html(response);
                    $(target).collapse('show');
                }
            })
        })
    }
}
Site.instance.paginas["MeusPedidos"] = MeusPedidos.instance;