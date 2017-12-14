import * as React from 'react';
import { Component, PropTypes } from 'react';
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
                { props.invoices.map(invoiceYear =>
                    <div>
                        <h4>{ invoiceYear.year}</h4>
                        { invoiceYear.months.map(invoiceMonth =>
                            <div>
                                <h5>{ invoiceMonth.month }</h5>
                                <table className="table">
                                    <thead>
                                        <tr>
                                            <th>Bestemmeling</th>
                                            <th>Bedrag</th>
                                            <th>Status</th>
                                            <th>Acties</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        {invoiceMonth.invoices.map(x => 
                                            <tr key={x.id}>
                                                <td><Link to={`/Invoices/Invoice/${x.id}`}>{x.receivingCompany}</Link></td>
                                                <td>{ x.total }</td>
                                                <td>{ InvoiceStatus[x.status] }</td>
                                                <td>
                                                    <a href={x.link} title="Bekijk pdf" target="_blank"><i className="fa fa-file-pdf-o action-icon"></i></a>
                                                    { x.status === InvoiceStatus.Sent && 
                                                        <a href="#" onClick={() => props.dispatch(Actions.updateInvoiceStatus(x, InvoiceStatus.Paid))}><i className="fa fa-credit-card action-icon" title="Markeer als betaald"></i></a>
                                                    }
                                                </td>
                                            </tr>
                                        )}
                                    </tbody>
                                </table>
                            </div>
                        )}
                    </div>
                )}
            </div>
        )
    }
}