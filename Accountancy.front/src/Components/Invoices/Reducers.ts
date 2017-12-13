import Action from 'Infrastructure/Action';
import newState from 'Infrastructure/newState';
import { GetInvoiceDone } from './Actions';
import { Invoice } from 'Components/Invoices/models';

export class State {
    invoice?: Invoice;
}

const initialState: State = {
}

export default function reducers(oldState = initialState, action: Action<any>) {
    switch (action.type) {
        case GetInvoiceDone: {
            const data = action.payload as GetInvoiceDone;
            return newState(initialState, state => {
                state.invoice = data.invoice;
            });
        }
        default:
            return oldState;
    }
}