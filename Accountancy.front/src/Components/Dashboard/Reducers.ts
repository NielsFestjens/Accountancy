import Action from 'Infrastructure/Action';
import newState from 'Infrastructure/newState';
import * as actions from './Actions';
import { InvoiceDto, InvoiceYear } from 'Components/Dashboard/models';
import { apiUri } from 'config';
import { Invoice } from 'Components/Invoices/models';

export class State {
    invoices: InvoiceDto[];
    invoiceYears: InvoiceYear[];
}

const initialState: State = {
    invoices: [],
    invoiceYears: []
}

export default function reducers(oldState = initialState, action: any) {
    switch (action.type) {

        case actions.DASHBOARD_FETCHED_INVOICES: {
            const data = action.payload as actions.DASHBOARD_FETCHED_INVOICES;
            return newState(oldState, state => {
                data.invoices.forEach(invoice => invoice.link = `${apiUri}/Invoices/PrintPdf?id=${invoice.id}`);
                state.invoices = data.invoices;
                state.invoiceYears = groupBy(data.invoices, x => x.year)
                    .map(x => ({
                        year: x.key,
                        months: groupBy(x.items, x => x.month)
                            .map(x => ({ 
                                month: x.key, 
                                invoices: x.items
                            }))
                    }));
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

class Group<TKey, TItem> {
    key: TKey;
    items: TItem[];
}

function groupBy<TItem, TKey, TResult>(list: TItem[], keyGetter: (item: TItem) => TKey): Group<TKey, TItem>[] {
    const groups: Group<TKey, TItem>[] = [];
    list.forEach((item) => {
        const key = keyGetter(item);
        const collection = groups.filter(x => x.key === key)[0];
        if (!collection) {
            groups.push({
                key,
                items: [item]
            });
        } else {
            collection.items.push(item);
        }
    });
    return groups;
}