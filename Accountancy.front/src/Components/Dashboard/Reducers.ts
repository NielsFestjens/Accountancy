import Action from 'Infrastructure/Action';
import newState from 'Infrastructure/newState';
import * as actions from './Actions';
import { Invoice } from 'Components/Dashboard/models';

class State {
    invoices: Invoice[]
}

const initialState: State = {
    invoices: []
}

export default function auth(oldState = initialState, action: any) {
    switch (action.type) {

        case actions.DASHBOARD_FETCHED_INVOICES:
            const data = action.payload as actions.DASHBOARD_FETCHED_INVOICES;
            return newState(oldState, state => {
                state.invoices = data.invoices;
            })

        default:
            return oldState;

    }
}