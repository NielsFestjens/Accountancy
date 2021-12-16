import { InvoiceStatus } from "Components/Invoices/models";

export class InvoiceMonth {
    month!: number;
    invoices!: InvoiceDto[];
}

export class InvoiceYear {
    year!: number;
    months!: InvoiceMonth[];
}

export class InvoiceDto {
    id!: number;
    year!: number;
    month!: number;
    receivingCompany!: string;
    status!: InvoiceStatus;
    total!: number;
    link!: string;
}