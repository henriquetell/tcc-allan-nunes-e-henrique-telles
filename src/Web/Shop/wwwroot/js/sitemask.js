/*eslint-disable */
(function (factory) {
    //https://github.com/umdjs/umd/blob/master/templates/jqueryPlugin.js
    if (typeof define === 'function' && define.amd) {
        define(['jquery'], factory);
    } else if (typeof module === 'object' && module.exports) {
        module.exports = function (root, jQuery) {
            if (jQuery === undefined) {
                if (typeof window !== 'undefined') {
                    jQuery = require('jquery');
                }
                else {
                    jQuery = require('jquery')(root);
                }
            }
            factory(jQuery);
            return jQuery;
        };
    } else {
        // Browser globals
        factory(jQuery);
    }
}(function ($) {
    $.fn.siteMask = function () {
        var target = this;

        $.jMaskGlobals.dataMask = false;
        $.jMaskGlobals.watchInterval = Math.pow(2, 31);

        var p = {
            target: target,
            mascaras: {},
            registrarMascara: function (nome, fnMascara) {
                this.mascaras[nome] = fnMascara;
            },
            aplicarMascara: function (nome) {
                var self = this;

                $("[data-mascara='" + nome + "']:not([data-mascara-aplicado])", this.target).each(function () {
                    var target = $(this);
                    self.mascaras[nome](target);

                    var dataMask = target.data("mask");

                    if (dataMask && dataMask.options && (dataMask.options.removeOnSubmit === true || $.isFunction(dataMask.options.removeOnSubmit))) {
                        target.parents("form").on("submit", function () {
                            if ($.isFunction($(this).valid) && !$(this).valid())
                                return;
                            if ($.isFunction(dataMask.options.removeOnSubmit)) {
                                dataMask.options.removeOnSubmit(target);
                            }
                            else {
                                target.unmask();
                            }

                        });
                    }

                    if (dataMask && dataMask.options && dataMask.options.clearIncomplete) {
                        target.on("blur", function () {
                            if (target.val().length !== dataMask.mask.length) {
                                target.val("");
                            }
                        });
                    }

                    $(this).attr("data-mascara-aplicado", "1");
                });
            },
            init: function () {
                for (var mascara in this.mascaras) {
                    if (this.mascaras.hasOwnProperty(mascara)) {
                        this.aplicarMascara(mascara);
                    }
                }
            }
        };

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Mascaras
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Propriedades adicionais:
        //
        // removeOnSubmit: remover a macara do input quando ocorrer o submit pelo fome
        // clearIncomplete: limpar o campo quando o valor inserido não corresponder a mascara
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        p.registrarMascara("telefone", function (target) {
            var maskTelefone = function (val) {
                return val.replace(/\D/g, '').length === 9 ? '00000-0000' : '0000-00009';
            };
            var maskTelefoneOptions = {
                onKeyPress: function (val, e, field, options) {
                    field.mask(maskTelefone.apply({}, arguments), options);
                },
                placeholder: "_____-____",
                removeOnSubmit: true
            };
            target.mask(maskTelefone, maskTelefoneOptions);
        });

        p.registrarMascara("ddd-telefone", function (target) {
            var maskTelefone = function (val) {
                return val.replace(/\D/g, '').length === 9 ? '(00) 00000-0000' : '(00) 0000-00009';
            };
            var maskTelefoneOptions = {
                onKeyPress: function (val, e, field, options) {
                    field.mask(maskTelefone.apply({}, arguments), options);
                },
                placeholder: "(__) _____-____",
                removeOnSubmit: true
            };
            target.mask(maskTelefone, maskTelefoneOptions);
        });

        p.registrarMascara("data", function (target) {
            target.mask('00/00/0000', {
                clearIncomplete: true,
                placeholder: "__/__/____"
            });
        });

        p.registrarMascara("validade", function (target) {
            target.mask('00/0000', {
                clearIncomplete: true,
                placeholder: "__/____"
            });
        });

        p.registrarMascara("cnpj", function (target) {
            target.mask('00/00/0000', {
                clearIncomplete: true,
                placeholder: "___.___.___/____-__"
            });
        });

        p.registrarMascara("cpf", function (target) {
            target.mask('000.000.000-00', {
                removeOnSubmit: true,
                clearIncomplete: true,
                placeholder: "__.___.___-__"
            });
        });

        p.registrarMascara("cpf-cnpj", function (target) {
            var maskCpfCnpj = function (val) {
                return val.replace(/\D/g, '').length > 11 ? '00.000.000/0000-00' : '000.000.000-009';
            };
            var maskCpfCnpjOptions = {
                onKeyPress: function (val, e, field, options) {
                    field.mask(maskCpfCnpj(val), options);
                },
                removeOnSubmit: true,
                placeholder: "___.___.___-__"
            };
            target.mask(maskCpfCnpj, maskCpfCnpjOptions);
        });

        p.registrarMascara("cep", function (target) {
            target.mask("00000-000", {
                removeOnSubmit: true,
                clearIncomplete: true,
                placeholder: "_____-___"
            });
        });

        p.registrarMascara("hora", function (target) {
            target.mask("00:00", {
                removeOnSubmit: false,
                clearIncomplete: true,
                placeholder: "__:__"
            });
        });

        p.registrarMascara("numerico", function (target) {
            var m = "99999999999999";
            var maxLength = target.prop("maxlength");
            if (typeof maxLength === "number" && maxLength > 0)
                m = m.substr(0, maxLength);

            target.mask(m, {
                removeOnSubmit: false,
                clearIncomplete: false
            });
        });

        p.registrarMascara("numero-negativo", function (target) {
            target.mask("Z99999999999999", {
                translation: {
                    'Z': {
                        pattern: /[-]/,
                        optional: true
                    }
                },
                removeOnSubmit: false,
                clearIncomplete: false
            });
        });

        p.registrarMascara("moeda", function (target) {
            target.mask("#.##0,00", {
                removeOnSubmit: false,
                clearIncomplete: false,
                reverse: true
            });
        });

        p.registrarMascara("porcentagem", function (target) {
            target.mask("000,##", {
                removeOnSubmit: false,
                clearIncomplete: false,
                reverse: true
            });
        });

        p.registrarMascara("cartao-credito", function (target) {
            var mascaras = {
                padrao: function (field) {
                    field.mask("0000 0000 0000 0000", {
                        placeholder: "____ ____ ____ ____",
                        removeOnSubmit: function (t) {
                            var v = $(t).val().replace(/[^0-9]/g, "");
                            $(t).val(v);
                        }
                    });
                },
                amex: function (field) {
                    var amexMask = function (val) {
                        return val.replace(/\D/g, "").length === 15 ? "0000 000000 000009" : "0000 0000 0000 0000";
                    };
                    field.mask(amexMask, {
                        onKeyPress: function (val, e, field, options) {
                            field.mask(amexMask.apply({}, arguments), options);
                        },
                        removeOnSubmit: function (t) {
                            var v = $(t).replace(/[^0-9]/g, "");
                            $(t).val(v);
                        }
                    });
                },
                setMask: function (field) {
                    field = field || this;
                    var length = $(field).val().replace(/\D/g, "").length;

                    if (length === 15) mascaras.amex($(field));
                    else mascaras.padrao($(field));
                }
            };

            mascaras.setMask(target);

            target.on("focus focusout", function () { mascaras.setMask(this); })
                .on("focusout", function () {
                    var length = $(this).val().replace(/\D/g, "").length;
                    if (length < 15)
                        $(this).val("");
                });
        });

        p.registrarMascara("cartao-credito-cco", function (target) {
            target.mask("0009", {
                removeOnSubmit: false
            });
        });

        ////Inicializar
        p.init();
    };
    }));
/*eslint-enable */