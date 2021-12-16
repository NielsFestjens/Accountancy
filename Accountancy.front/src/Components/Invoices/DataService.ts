import ApiCaller from 'Infrastructure/ApiCaller';
import { apiUri } from 'config';

const apiCaller = new ApiCaller(apiUri + 'Invoices/');

export const getInvoice = (onError: (message: string) => void, id: number) => apiCaller.get(onError, 'getInvoice', { id });