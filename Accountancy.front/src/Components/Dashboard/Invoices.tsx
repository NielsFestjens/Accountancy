import * as React from 'react';
import { Component, ReactFragment } from 'react';
import { Link } from 'react-router-dom';

import { InvoiceDto, InvoiceYear } from './models';
import * as Actions from './Actions';
import { InvoiceStatus } from 'Components/Invoices/models';

export interface IProps {
    dispatch: (action: any) => void;
    invoices: InvoiceYear[]
}

export default class Invoices extends Component<IProps> {

    render() {
        const props = this.props

        return (
            <div>
                <h3>Facturen</h3>
                <table className="table">
                    <thead>
                        <tr>
                            <th>Jaar</th>
                            <th>Maand</th>
                            <th>Bestemmeling</th>
                            <th>Bedrag</th>
                            <th>Status</th>
                            <th>Acties</th>
                        </tr>
                    </thead>
                    <tbody>
                        { props.invoices.map(invoiceYear =>
                            <React.Fragment key={`year_${invoiceYear.year}`}>
                                <tr>
                                    <td>{invoiceYear.year}</td>
                                    <td></td>
                                    <td>Totaal</td>
                                    <td>{invoiceYear.months.map(x => x.invoices.map(i => i.total).reduce((a, b) => a + b, 0)).reduce((a, b) => a + b, 0).toFixed(2)}</td>
                                </tr>
                                { invoiceYear.months.map(invoiceMonth =>
                                    <React.Fragment key={`month_${invoiceYear.year}_${invoiceMonth.month}`}>
                                        <tr>
                                            <td></td>
                                            <td>{invoiceMonth.month}</td>
                                            <td>Totaal</td>
                                            <td>{invoiceMonth.invoices.map(x => x.total).reduce((a, b) => a + b, 0).toFixed(2)}</td>
                                        </tr>
                                        {...invoiceMonth.invoices.map(x => 
                                            <tr key={x.id}>
                                                <td></td>
                                                <td></td>
                                                <td><Link to={`/Invoices/Invoice/${x.id}`}>{x.receivingCompany}</Link></td>
                                                <td>{ x.total.toFixed(2) }</td>
                                                <td>{ InvoiceStatus[x.status] }</td>
                                                <td>
                                                    <a href={x.link} title="Bekijk pdf" target="_blank"><i className="fa fa-file-pdf-o action-icon"></i></a>
                                                    { x.status === InvoiceStatus.Sent && 
                                                        <a href="#" onClick={() => props.dispatch(Actions.updateInvoiceStatus(x, InvoiceStatus.Paid))}><i className="fa fa-credit-card action-icon" title="Markeer als betaald"></i></a>
                                                    }
                                                </td>
                                            </tr>
                                        )}
                                    </React.Fragment>
                                )}
                            </React.Fragment>
                        )}
                </tbody>
            </table>
            </div>
        )
    }
}

