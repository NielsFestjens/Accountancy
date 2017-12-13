import Action from 'Infrastructure/Action';
import newState from 'Infrastructure/newState';
import * as actions from './Actions';
import { InvoiceDto } from 'Components/Dashboard/models';
import { apiUri } from 'config';

export class State {
    invoices: InvoiceDto[]
}

const initialState: State = {
    invoices: []
}

export default function reducers(oldState = initialState, action: any) {
    switch (action.type) {

        case actions.DASHBOARD_FETCHED_INVOICES: {
            const data = action.payload as actions.DASHBOARD_FETCHED_INVOICES;
            return newState(oldState, state => {
                data.invoices.forEach(invoice => invoice.link = `${apiUri}/Invoices/PrintPdf?id=${invoice.id}`);
                state.invoices = data.invoices;
            });
        }
        case actions.DASHBOARD_UPDATED_INVOICE_STATUS: {
            const data = action.payload as actions.DASHBOARD_UPDATED_INVOICE_STATUS;
            return newState(oldState, state => {
                state.invoices.filter(x => x.id === data.invoice.id)[0].status = data.status;
            });
        }
        default:
            return oldState;

    }
}