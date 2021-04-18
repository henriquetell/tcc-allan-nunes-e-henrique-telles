var Site = (function () {
    function Site() {
        this.paginas = {};
        this.layout = {};
        this.onLoad = [];
    }
    Object.defineProperty(Site, "instance", {
        get: function () {
            return this.ins || (this.ins = new this());
        },
        enumerable: true,
        configurable: true
    });
    Site.prototype.init = function () {
        $((document)).siteMask();
        if ($.validator) {
            $.validator.methods['range'] = function (value, element, param) {
                var globalizedValue = value.replace(",", ".");
                return this.optional(element) || (globalizedValue >= param[0] && globalizedValue <= param[1]);
            };
            $.validator.methods['number'] = function (value, element) {
                return this.optional(element) || /^-?(?:\d+|\d{1,3}(?:[\s\.,]\d{3})+)(?:[\.,]\d+)?$/.test(value);
            };
        }
        $(function (e) {
            for (var prop in Site.instance.layout) {
                if (Site.instance.layout.hasOwnProperty(prop)) {
                    var cia = Site.instance.layout[prop];
                    if (cia && cia.init && $.isFunction(cia.init))
                        cia.init(e);
                }
            }
            Site.bindPagina($("[data-pagina]"), e);
        });
    };
    Site.bindPagina = function (target, e) {
        target.each(function (index, item) {
            if (!$(item).data("pagina-carregado")) {
                $(item).data("pagina-carregado", true);
                var cia = Site.instance.paginas[$(item).data("pagina")];
                if (cia && cia.init && $.isFunction(cia.init))
                    cia.init(e, $(item).data("init"));
            }
        });
    };
    Site.bindSelect = function (event, url, sender, target, data) {
        if (!url || !sender || !target)
            return;
        $.ajax({
            ajaxLoader: false,
            cache: false,
            method: "POST",
            data: data,
            async: true,
            url: url,
            success: function (response) {
                var html = "";
                if ($(target).data("selecione") ||
                    $(target).data("selecione") === null ||
                    $(target).data("selecione") === undefined)
                    html = "<option value=\"\">Selecione...</option>";
                if (response != null && response !== undefined && response.length > 0) {
                    $(target).prop("disabled", false);
                    var grupos = [];
                    $(response).each(function (index, item) {
                        if (item.group && item.group.name && grupos.indexOf(item.group.name) === -1) {
                            grupos.push(item.group.name);
                        }
                    });
                    if (grupos.length === 0) {
                        $(response).each(function (index, item) {
                            var selecionado = $(target).data("value") == item.value ||
                                ($(target).data("value") &&
                                    $(target).data("value").indexOf(parseInt(item.value)) !== -1)
                                ? "selected=\"selected\""
                                : "";
                            html += "<option value=\"" + item.value + "\"" + selecionado + ">" + item.text + "</option>";
                        });
                    }
                    else {
                        $(grupos).each(function (g, grupo) {
                            html += "<optgroup label=\"" + grupo + "\">";
                            $(response).each(function (index, item) {
                                if (item.group && item.group.name && item.group.name === grupo) {
                                    var selecionado = $(target).data("value") == item.value ||
                                        ($(target).data("value") &&
                                            $(target).data("value").indexOf(parseInt(item.value)) !== -1)
                                        ? "selected=\"selected\""
                                        : "";
                                    html += "<option value=\"" + item.value + "\"" + selecionado + ">" + item.text + "</option>";
                                }
                            });
                            html += "</optgroup>";
                        });
                    }
                }
                else if ($(target).attr("chosen") !== undefined) {
                    $(target).prop("disabled", $(target).data("disabled"));
                }
                $(target).html(html);
                if ($(target).attr("chosen") !== undefined) {
                    $(target).trigger("chosen:updated");
                }
                $(target).trigger("change");
            }
        });
    };
    return Site;
}());
//# sourceMappingURL=Site.js.map
var Carrinho = (function () {
    function Carrinho() {
    }
    Object.defineProperty(Carrinho, "instance", {
        get: function () {
            return this.ins || (this.ins = new this());
        },
        enumerable: true,
        configurable: true
    });
    Carrinho.prototype.init = function (ev) {
        var self = this;
        $(".esh-basket-minus").on("click", function (event) {
            event.preventDefault();
            self.post($(this));
        });
        $(".esh-basket-plus").on("click", function (event) {
            event.preventDefault();
            self.post($(this));
        });
        $(".esh-basket-remove").on("click", function (event) {
            event.preventDefault();
            self.post($(this));
        });
        $(".esh-basket-items").each(function (index, item) {
            var value = parseInt($(".esh-basket-input").val());
            if (value > 1) {
                $(".esh-basket-minus", item).show();
            }
            else {
                $(".esh-basket-minus", item).hide();
            }
        });
    };
    Carrinho.prototype.post = function (sender) {
        var url = sender.attr("href");
        if (url) {
            var data = {
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
                beforeSend: function () {
                    sender.prop("disabled", true);
                },
                success: function () {
                    window.location.reload();
                },
                error: function (err) {
                    alert(err.responseText);
                },
                complete: function () {
                    sender.prop("disabled", false);
                }
            });
        }
    };
    return Carrinho;
}());
Site.instance.paginas["Carrinho"] = Carrinho.instance;
//# sourceMappingURL=Carrinho.js.map
var Checkout = (function () {
    function Checkout() {
    }
    Object.defineProperty(Checkout, "instance", {
        get: function () {
            return this.ins || (this.ins = new this());
        },
        enumerable: true,
        configurable: true
    });
    Checkout.prototype.init = function (ev) {
        var self = this;
        PagSeguroDirectPayment.setSessionId($("#Pagamento_IdSessao").val());
        $("#Pagamento_NumeroCartao").on("change", function () {
            var cartao = $(this).val();
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
            var sender = $(this);
            sender.prop("disabled", true);
            PagSeguroDirectPayment.onSenderHashReady(function (response) {
                if (response.status == 'error') {
                    console.log(response.message);
                    sender.prop("disabled", false);
                    return false;
                }
                $("input#Pagamento_Hash").val(response.senderHash);
                Checkout.getCardToken(sender);
            });
        });
    };
    Checkout.getCardToken = function (sender) {
        PagSeguroDirectPayment.createCardToken({
            cardNumber: $("input#Pagamento_NumeroCartao").val(),
            brand: $("input#Pagamento_Bandeira").val(),
            cvv: $("input#Pagamento_Cvv").val(),
            expirationMonth: $("select#Pagamento_MesValidade").val(),
            expirationYear: $("select#Pagamento_AnoValidade").val(),
            success: function (response) {
                $("input#Pagamento_CardToken").val(response.card.token);
                $("form").data("valid", true);
                $("form").submit();
            },
            error: function (response) {
                console.log(response);
                sender.prop("disabled", false);
                $("#CardToken").val('');
            }
        });
    };
    Checkout.getBrand = function (cardBin) {
        PagSeguroDirectPayment.getBrand({
            cardBin: cardBin,
            success: function (response) {
                $("#Pagamento_Bandeira").val(response.brand.name);
                Checkout.getInstallments(response.brand.name);
            },
            error: function (response) {
                console.log(response);
            }
        });
    };
    Checkout.getInstallments = function (brand) {
        var valorTotal = $("input[type='hidden']#ValorTotal").val();
        var quantidadeMaximaParcelamento = parseInt($("input#QuantidadeMaximaParcelamento").val());
        PagSeguroDirectPayment.getInstallments({
            amount: valorTotal.replace(',', '.'),
            maxInstallmentNoInterest: quantidadeMaximaParcelamento,
            brand: brand,
            success: function (resp) {
                if (!resp.error) {
                    var html_1 = "";
                    var select = $("#Pagamento_QuantidadeParcelas");
                    var value_1 = parseInt(select.data("value"));
                    $(resp.installments[brand]).each(function (index, item) {
                        if (item.quantity <= quantidadeMaximaParcelamento) {
                            var juros = item.interestFree ? "sem juros" : "com juros";
                            var selected = value_1 === item.quantity ? "selected" : "";
                            html_1 += "<option value=\"" + item.quantity + "\"" + selected + ">" + item.quantity + "x de " + item.installmentAmount + " " + juros + "</option>";
                        }
                    });
                    select.html(html_1);
                }
            },
            error: function (resp) {
                console.log(resp);
            }
        });
    };
    return Checkout;
}());
Site.instance.paginas["Checkout"] = Checkout.instance;
//# sourceMappingURL=Checkout.js.map
var FaleConosco = (function () {
    function FaleConosco() {
    }
    Object.defineProperty(FaleConosco, "instance", {
        get: function () {
            return this.ins || (this.ins = new this());
        },
        enumerable: true,
        configurable: true
    });
    FaleConosco.prototype.init = function (ev) {
        var self = this;
        $("form#FaleConosco").submit(function (event) {
            event.preventDefault();
            var action = $(this).attr('action');
            var form = $(this);
            if (!form.valid() || !action) {
                return false;
            }
            var inputs = $('input:not([type="hidden"])', form);
            var textArea = $('textarea', form);
            var formData = new FormData(this);
            $.ajax({
                url: action,
                type: 'POST',
                data: formData,
                beforeSend: function () {
                    inputs.prop('disabled', true);
                    textArea.prop('disabled', true);
                    $('button', form).prop('disabled', true);
                },
                success: function (data) {
                    inputs.val('');
                    textArea.val('');
                    alert(data);
                },
                error: function (err) {
                    alert(err.responseText);
                },
                complete: function () {
                    inputs.prop('disabled', false);
                    textArea.prop('disabled', false);
                    $('button', form).prop('disabled', false);
                },
                cache: false,
                contentType: false,
                processData: false
            });
        });
    };
    return FaleConosco;
}());
Site.instance.paginas["FaleConosco"] = FaleConosco.instance;
//# sourceMappingURL=FaleConosco.js.map
var Login = (function () {
    function Login() {
    }
    Object.defineProperty(Login, "instance", {
        get: function () {
            return this.ins || (this.ins = new this());
        },
        enumerable: true,
        configurable: true
    });
    Login.prototype.init = function (ev) {
        var self = this;
    };
    return Login;
}());
Site.instance.paginas["Login"] = Login.instance;
//# sourceMappingURL=Login.js.map
var MeusPedidos = (function () {
    function MeusPedidos() {
    }
    Object.defineProperty(MeusPedidos, "instance", {
        get: function () {
            return this.ins || (this.ins = new this());
        },
        enumerable: true,
        configurable: true
    });
    MeusPedidos.prototype.init = function (ev) {
        var self = this;
        $('#meus-pedidos').collapse({
            toggle: false
        });
        $('#meus-pedidos a.esh-orders-link').on('click', function (event) {
            var action = $(this).data('action');
            var target = $(this).data('target');
            $.ajax({
                async: false,
                method: 'GET',
                url: action,
                success: function (response) {
                    $(target).html(response);
                    $(target).collapse('show');
                }
            });
        });
    };
    return MeusPedidos;
}());
Site.instance.paginas["MeusPedidos"] = MeusPedidos.instance;
//# sourceMappingURL=MeusPedidos.js.map
var MinhaConta = (function () {
    function MinhaConta() {
    }
    Object.defineProperty(MinhaConta, "instance", {
        get: function () {
            return this.ins || (this.ins = new this());
        },
        enumerable: true,
        configurable: true
    });
    MinhaConta.prototype.init = function (ev) {
        var self = this;
        $("#AlterarSenha").on("change", function () {
            if ($(this).is(":checked")) {
                $("[alterar-senha]").show();
            }
            else {
                $("[alterar-senha]").hide();
            }
        }).trigger("change");
    };
    return MinhaConta;
}());
Site.instance.paginas["MinhaConta"] = MinhaConta.instance;
//# sourceMappingURL=MinhaConta.js.map
var Produto = (function () {
    function Produto() {
    }
    Object.defineProperty(Produto, "instance", {
        get: function () {
            return this.ins || (this.ins = new this());
        },
        enumerable: true,
        configurable: true
    });
    Produto.prototype.init = function (ev) {
        var self = this;
        $("form#produto").on("submit", function (event) {
            var val = $('[name="IdProdutoSku"]:checked').val();
            return val > 0;
        });
    };
    return Produto;
}());
Site.instance.paginas["Produto"] = Produto.instance;
//# sourceMappingURL=Produto.js.map