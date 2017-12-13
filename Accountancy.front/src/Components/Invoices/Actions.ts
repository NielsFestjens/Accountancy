import Action from 'Infrastructure/Action';
import * as notifications from 'Components/Blocks/Notifications/Actions';
import * as DataService from './DataService';
import { Invoice } from 'Components/Invoices/models';

export const GetInvoiceDone = 'Invoices_getInvoiceDone';
export type GetInvoiceDone = { invoice: Invoice };
export const getInvoiceDone = (invoice: Invoice): Action<GetInvoiceDone> => ({ type: GetInvoiceDone, payload: { invoice }})

export function fetchInvoice(id: number) {
    return (dispatch: (action: any) => void) => {
        DataService.getInvoice(id).then(result => {
            dispatch(getInvoiceDone(result.content));
        })
    }
}