import Action from 'Infrastructure/Action';
import newState from 'Infrastructure/newState';
import * as actions from './Actions';
import { InvoiceDto } from 'Components/Dashboard/models';
import { apiUri } from 'config';

class State {
    invoices: InvoiceDto[]
}

const initialState: State = {
    invoices: []
}

export default function reducers(oldState = initialState, action: any) {
    switch (action.type) {

        case actions.DASHBOARD_FETCHED_INVOICES:
            const data = action.payload as actions.DASHBOARD_FETCHED_INVOICES;
            return newState(oldState, state => {
                data.invoices.forEach(invoice => invoice.link = `${apiUri}/PrintPdf?id=${invoice.id}`);
                state.invoices = data.invoices;
            })

        default:
            return oldState;

    }
}