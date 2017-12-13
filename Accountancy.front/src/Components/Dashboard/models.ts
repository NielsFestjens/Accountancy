import { InvoiceStatus } from "Components/Invoices/models";

export class InvoiceDto {
    id: number;
    name: string;
    status: InvoiceStatus;
    link: string;
}