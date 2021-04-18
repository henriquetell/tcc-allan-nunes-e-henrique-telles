// Type definitions for PagSeguro
declare class PagSeguroDirectPayment {
    constructor();
    static setSessionId(value: any): void;
    static onSenderHashReady(value: any): void;
    static createCardToken(value: any): void;
    static getBrand(value: any): void;
    static getInstallments(value: any): void;
}

export = PagSeguroDirectPayment;
export as namespace PagSeguroDirectPayment;