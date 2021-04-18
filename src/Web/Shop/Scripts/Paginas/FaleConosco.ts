class FaleConosco {
    private static ins: FaleConosco;
    static get instance() {
        return this.ins || (this.ins = new this());
    }
    init(ev: any): void {
        const self = this;

        $("form#FaleConosco").submit(function (event) {
            event.preventDefault();
            const action = $(this).attr('action');
            const form = $(this);
            if (!form.valid() || !action) {
                return false;
            }
            const inputs = $('input:not([type="hidden"])', form);
            const textArea = $('textarea', form);
            var formData = new FormData(this as HTMLFormElement);
            $.ajax({
                url: action,
                type: 'POST',
                data: formData,
                beforeSend: function () {
                    inputs.prop('disabled', true);
                    textArea.prop('disabled', true);
                    $('button', form).prop('disabled', true);
                },
                success: function (data: any) {
                    inputs.val('');
                    textArea.val('');
                    alert(data);
                },
                error: function (err: any) {
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
    }
}
Site.instance.paginas["FaleConosco"] = FaleConosco.instance;