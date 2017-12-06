import ApiCaller from 'Infrastructure/ApiCaller';
import { apiUri } from 'config';
import { InvoiceDto } from './models';

let apiCaller = new ApiCaller(apiUri);

export function getDashboardInvoices(): Promise<InvoiceDto[]> {
    return apiCaller.get('dashboard/getInvoices').then(response => response.content);
}