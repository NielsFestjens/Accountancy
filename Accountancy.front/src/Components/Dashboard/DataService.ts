import ApiCaller from 'Infrastructure/ApiCaller';
import { apiUri } from 'config';
import { InvoiceDto } from './models';
import { InvoiceStatus } from 'Components/Invoices/models';

const apiCaller = new ApiCaller(apiUri + 'Dashboard/');

export const getDashboardInvoices = (): Promise<InvoiceDto[]> => apiCaller.get('getInvoices').then(response => response.content);
export const updateInvoiceStatus = (invoice: InvoiceDto, status: InvoiceStatus): Promise<any> => apiCaller.post('updateInvoiceStatus', { id: invoice.id, status });