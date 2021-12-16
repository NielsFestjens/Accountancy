import { useState } from 'react';
import { useParams } from 'react-router';
import useAsyncEffect from "use-async-effect";
import { Invoice, InvoiceStatus } from 'Components/Invoices/models';
import * as DataService from './DataService';

export interface IProps {
    addNotificationError: (message: string) => void;
}

const Detail = (props: IProps) => {
    const { addNotificationError } = props;

    const { id } = useParams();

    const [invoice, setInvoice] = useState<Invoice>();

    useAsyncEffect(async (isMounted) => {
        if (!id)
            return;
        
        const result = await DataService.getInvoice(addNotificationError, parseFloat(id));
        if (isMounted())
            setInvoice(result.content);
    }, [id]);

    return (
        <div>
            <h2>Factuur detail</h2>
            { invoice && 
                <div>
                    Jaar: { invoice.year }<br />
                    Maand: { invoice.month }<br />
                    Status: { InvoiceStatus[invoice.status] }<br />
                    <table className="table">
                    <thead>
                        <tr>
                            <th>Omschrijving</th>
                            <th>Aantal</th>
                            <th>Prijs</th>
                            <th>Totaal</th>
                        </tr>
                    </thead>
                    <tbody>
                        {invoice.invoiceLines.map(x => 
                            <tr key={x.id}>
                                <td>{ x.description }</td>
                                <td>{ x.amount.toFixed(2) }</td>
                                <td>{ x.price.toFixed(2) }</td>
                                <td>{ (x.price * x.amount).toFixed(2) }</td>
                            </tr>
                        )}
                    </tbody>
                    </table>
                </div>
            }
        </div>
    );
}

export default Detail;