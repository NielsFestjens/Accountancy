import ApiCaller from 'Infrastructure/ApiCaller';
import { apiUri } from 'config';

let apiCaller = new ApiCaller(apiUri);

export function getDashboardInvoices() {
    return apiCaller.get('getDashboardInvoices')
}