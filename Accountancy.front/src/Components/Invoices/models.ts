export class Invoice {
    id!: number;
    status!: InvoiceStatus;
    year!: number;
    month!: number;
    date!: Date;
    invoiceLines!: InvoiceLine[]
}

export class InvoiceLine {
    id!: number;
    description!: string;
    amount!: number;
    price!: number;
    vatType!: VatType
}

export enum InvoiceStatus {
    Draft,
    Sent,
    Paid,
}

export enum VatType
{
    Vat21 = 21
}