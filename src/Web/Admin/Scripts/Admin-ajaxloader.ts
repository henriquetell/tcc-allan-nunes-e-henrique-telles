class Ajaxloader {
    private static ins: Ajaxloader;
    constructor() { }
    static get instance() {
        return this.ins || (this.ins = new this());
    }
    show(): void {
        if (!$("#ajaxLoader").is(":visible")) {
            $("#ajaxLoader").fadeIn("fast");
        }
    }
    hide(): void {
        if ($("#ajaxLoader").is(":visible")) {
            $("#ajaxLoader").fadeOut("fast");
        }
    }
    init(e: any): void {
        var that: any = this;

        $((document) as any).ajaxSend((event: JQuery.Event, xhr: any, options: any) => {
            if (options.ajaxLoader !== false)
                that.show();
        });

        $((document) as any).ajaxComplete((event: JQuery.Event, xhr: any, options: any) => {
            if (options.ajaxLoader !== false)
                that.hide();
        });

        $((document) as any).on("submit", "form", (e: any) => {
            if (e.isPropagationStopped())
                return;
            let self = this;
            let hide = $(e.target).data("hide");
            if (hide) {
                setTimeout(() => {
                    self.hide();
                }, parseInt(hide));
            }

            self.show();
        });

        $((document) as any).on("click", "a:not([data-ajax-loader='no'], [href^='#'], [target], [data-dialog-form], [data-pjax])", (e: any) => {
            if (e.isPropagationStopped() || e.ctrlKey || e.shiftKey)
                return;

            that.show();
        });

        that.hide();
    }
}
Admin.instance.layout["Ajaxloader"] = Ajaxloader.instance;
