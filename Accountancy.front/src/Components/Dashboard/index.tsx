import { useState } from 'react';
import useAsyncEffect from 'use-async-effect';
import Invoices from 'Components/Dashboard/Invoices';
import Jaaroverzicht from 'Components/Dashboard/Jaaroverzicht';
import { InvoiceDto, InvoiceYear } from './models';
import { InvoiceStatus } from 'Components/Invoices/models';
import * as DataService from './DataService';
import { apiUri } from 'config';
import { groupBy } from 'Infrastructure/GroupBy';

function groupByYearAndMonth(invoices: InvoiceDto[]) {
    return groupBy(invoices, x => x.year).map(x => ({
        year: x.key,
        months: groupBy(x.items, x => x.month).map(x => ({ 
            month: x.key, 
            invoices: x.items
        }))
    }));
}

type DashboardProps = {
    addNotificationError: (message: string) => void;
}

const Dashboard = (props: DashboardProps) => {
    const { addNotificationError } = props;
    const [invoices, setInvoices] = useState<InvoiceDto[]>([]);
    const [invoiceYears, setInvoiceYears] = useState<InvoiceYear[]>([]);
    
    useAsyncEffect(async (isMounted) => {
        const data = await DataService.getDashboardInvoices(addNotificationError);
        if (isMounted()) {
            data.forEach(invoice => invoice.link = `${apiUri}Invoices/PrintPdf?id=${invoice.id}`);
            setInvoices(data);
            setInvoiceYears(groupByYearAndMonth(data));
        }
    }, []);

    const updateInvoiceStatus = async (invoice: InvoiceDto, status: InvoiceStatus) => {
        await DataService.updateInvoiceStatus(addNotificationError, invoice, status);
        const newInvoices = [...invoices.filter(x => x.id !== invoice.id, {...invoice, status})];
        setInvoices(newInvoices);
        setInvoiceYears(groupByYearAndMonth(newInvoices));
    }

    return (
        <div>
            <h2>Le dashboard</h2>
            <Jaaroverzicht />
            <Invoices invoices={invoiceYears} updateInvoiceStatus={updateInvoiceStatus} />
        </div>
    );
}
export default Dashboard;