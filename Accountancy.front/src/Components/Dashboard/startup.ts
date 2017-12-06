import { getDashboardInvoices } from "./DataService";
import * as Actions from "./Actions";
import { History } from 'history';
import Action from "Infrastructure/Action";

export default (dispatch: (action: Action) => void) => {
    getDashboardInvoices().then(invoices => {
        dispatch(Actions.fetchedInvoices(invoices))
    });
}