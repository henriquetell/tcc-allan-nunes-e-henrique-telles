class Login {
    private static ins: Login;
    static get instance() {
        return this.ins || (this.ins = new this());
    }
    init(ev: any): void {
        const self = this;

        $("form input:not([type='checkbox'])").each((index: any, item: any) => {
            $(item).on("keyup", self.inputChange).trigger("keyup");
        });

    }

    inputChange(): void {
        let valid: boolean = true;
        $("form input:not([type='checkbox'])").each((index: any, item: any) => {
            if ($(item).val() === "" || $(item).val() === undefined) {
                valid = false;
            }
        });
        $("form button[type='submit']").prop("disabled", !valid);
    }
}
Admin.instance.paginas["Login"] = Login.instance;