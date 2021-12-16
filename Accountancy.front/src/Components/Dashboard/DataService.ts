import ApiCaller from 'Infrastructure/ApiCaller';
import { apiUri } from 'config';
import { InvoiceDto } from './models';
import { InvoiceStatus } from 'Components/Invoices/models';

const apiCaller = new ApiCaller(apiUri + 'Dashboard/');

export const getDashboardInvoices = (onError: (message: string) => void,): Promise<InvoiceDto[]> => apiCaller.get(onError, 'getInvoices').then(response => response.content);
export const updateInvoiceStatus = (onError: (message: string) => void, invoice: InvoiceDto, status: InvoiceStatus): Promise<any> => apiCaller.post(onError, 'updateInvoiceStatus', { id: invoice.id, status });