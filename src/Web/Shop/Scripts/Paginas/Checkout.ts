class Checkout {
    private static ins: Checkout;
    static get instance() {
        return this.ins || (this.ins = new this());
    }
    init(ev: any): void {
        const self = this;
        PagSeguroDirectPayment.setSessionId($("#Pagamento_IdSessao").val());

        $("#Pagamento_NumeroCartao").on("change", function () {
            var cartao = $(this).val() as string;
            if (!cartao || cartao.length < 6)
                return;
            Checkout.getBrand(cartao.replace(/\D/g, '').substring(0, 6));
        }).trigger("change");

        $("form").on("submit", function () {
            var valid = $(this).data("valid");
            if (valid)
                return true;
            return false;
        });
        $("button").on("click", function (event) {
            let sender = $(this);
            sender.prop("disabled", true);
            PagSeguroDirectPayment.onSenderHashReady(function (response: any) {
                if (response.status == 'error') {
                    console.log(response.message);
                    sender.prop("disabled", false);
                    return false;
                }
                $("input#Pagamento_Hash").val(response.senderHash);
                Checkout.getCardToken(sender);
            });
        });
    }

    static getCardToken(sender: any): void {
        PagSeguroDirectPayment.createCardToken({
            cardNumber: $("input#Pagamento_NumeroCartao").val(), // Número do cartão de crédito
            brand: $("input#Pagamento_Bandeira").val(), // Bandeira do cartão
            cvv: $("input#Pagamento_Cvv").val(), // CVV do cartão
            expirationMonth: $("select#Pagamento_MesValidade").val(), // Mês da expiração do cartão
            expirationYear: $("select#Pagamento_AnoValidade").val(), // Ano da expiração do cartão, é necessário os 4 dígitos.
            success: function (response: any) {
                $("input#Pagamento_CardToken").val(response.card.token);
                $("form").data("valid", true);
                $("form").submit();
            },
            error: function (response: any) {
                console.log(response);
                sender.prop("disabled", false);
                $("#CardToken").val('');
            }
        });
    }

    static getBrand(cardBin: string): void {
        PagSeguroDirectPayment.getBrand({
            cardBin: cardBin,
            success: function (response: any) {
                $("#Pagamento_Bandeira").val(response.brand.name);
                Checkout.getInstallments(response.brand.name);
            },
            error: function (response: any) {
                console.log(response);
            }
        });
    }

    static getInstallments(brand: string): void {
        var valorTotal = $("input[type='hidden']#ValorTotal").val() as string;
        var quantidadeMaximaParcelamento = parseInt($("input#QuantidadeMaximaParcelamento").val() as string);
        PagSeguroDirectPayment.getInstallments({
            amount: valorTotal.replace(',', '.'),
            maxInstallmentNoInterest: quantidadeMaximaParcelamento,
            brand: brand,
            success: function (resp: any) {
                if (!resp.error) {
                    let html = "";
                    let select = $("#Pagamento_QuantidadeParcelas");
                    let value = parseInt(select.data("value"));
                    $(resp.installments[brand]).each((index, item) => {
                        if (item.quantity <= quantidadeMaximaParcelamento) {
                            let juros = item.interestFree ? "sem juros" : "com juros";
                            let selected = value === item.quantity ? "selected" : "";
                            html += `<option value="${item.quantity}"${selected}>${item.quantity}x de ${item.installmentAmount} ${juros}</option>`;
                        }
                    });
                    select.html(html);
                }
            },
            error: function (resp: any) {
                console.log(resp);
            }
        });
    }
}
Site.instance.paginas["Checkout"] = Checkout.instance;