import ApiCaller from 'Infrastructure/ApiCaller';
import { apiUri } from 'config';

const apiCaller = new ApiCaller(apiUri + 'Invoices/');

export const getInvoice = (id: number) => apiCaller.get('getInvoice', { id });