export class InvoiceDto {
    id: number;
    name: string;
    status: InvoiceStatus;
    link: string;
}

export enum InvoiceStatus {
    Draft,
    Sent,
    Paid,
}