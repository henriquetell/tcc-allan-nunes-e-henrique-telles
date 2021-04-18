class Login {
    private static ins: Login;
    static get instance() {
        return this.ins || (this.ins = new this());
    }
    init(ev: any): void {
        const self = this;



    }
}
Site.instance.paginas["Login"] = Login.instance;