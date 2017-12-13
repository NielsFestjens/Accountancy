import Action from 'Infrastructure/Action';
import * as DataService from './DataService';
import * as notifications from 'Components/Blocks/Notifications/Actions';
import { InvoiceDto } from './models';
import { InvoiceStatus } from 'Components/Invoices/models';

export const DASHBOARD_FETCHED_INVOICES = 'DASHBOARD_FETCHED_INVOICES';
export type DASHBOARD_FETCHED_INVOICES = { invoices: InvoiceDto[] };
export const fetchedInvoices = (invoices: InvoiceDto[]): Action<DASHBOARD_FETCHED_INVOICES> => ({ type: DASHBOARD_FETCHED_INVOICES, payload: { invoices } });

export function fetchInvoices() {
    return (dispatch: (action: Action<any>) => void) => {
        DataService.getDashboardInvoices().then(result => {
            dispatch(fetchedInvoices(result));
        })
    }
}

export const DASHBOARD_UPDATED_INVOICE_STATUS = 'DASHBOARD_UPDATED_INVOICE_STATUS';
export type DASHBOARD_UPDATED_INVOICE_STATUS = { invoice: InvoiceDto, status: InvoiceStatus };
export const updatedInvoiceStatus = (invoice: InvoiceDto, status: InvoiceStatus): Action<DASHBOARD_UPDATED_INVOICE_STATUS> => ({ type: DASHBOARD_UPDATED_INVOICE_STATUS, payload: { invoice, status } });

export function updateInvoiceStatus(invoice: InvoiceDto, status: InvoiceStatus) {
    return (dispatch: (action: Action<any>) => void) => {
        DataService.updateInvoiceStatus(invoice, status).then(result => {
            dispatch(updatedInvoiceStatus(invoice, status));
        })
    }
}