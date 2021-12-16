import React from 'react';
import { Link } from 'react-router-dom';
import {  InvoiceDto, InvoiceYear } from './models';
import { InvoiceStatus } from 'Components/Invoices/models';

interface IProps {
    invoices: InvoiceYear[];
    updateInvoiceStatus: (invoice: InvoiceDto, status: InvoiceStatus) => void;
}

const Invoices = (props: IProps) => {

    const { invoices, updateInvoiceStatus } = props;

    return (
        <div>
            <h3>Facturen</h3>
            <table className="table">
                <thead>
                    <tr>
                        <th style={{width: "70px"}}>Jaar</th>
                        <th style={{width: "70px"}}>Maand</th>
                        <th>Bestemmeling</th>
                        <th>Bedrag</th>
                        <th>Status</th>
                        <th>Acties</th>
                    </tr>
                </thead>
                <tbody>
                    { invoices.map(invoiceYear =>
                        <React.Fragment key={`year_${invoiceYear.year}`}>
                            <tr>
                                <td><b>{invoiceYear.year}</b></td>
                                <td></td>
                                <td><b>Totaal</b></td>
                                <td><b>{invoiceYear.months.map(x => x.invoices.map(i => i.total).reduce((a, b) => a + b, 0)).reduce((a, b) => a + b, 0).toFixed(2)}</b></td>
                            </tr>
                            { invoiceYear.months.map(invoiceMonth =>
                                <React.Fragment key={`month_${invoiceYear.year}_${invoiceMonth.month}`}>
                                    <tr>
                                        <td></td>
                                        <td><b>{invoiceMonth.month}</b></td>
                                        <td><b>Totaal</b></td>
                                        <td><b>{invoiceMonth.invoices.map(x => x.total).reduce((a, b) => a + b, 0).toFixed(2)}</b></td>
                                    </tr>
                                    {invoiceMonth.invoices.map(x => 
                                        <tr key={x.id}>
                                            <td></td>
                                            <td></td>
                                            <td><Link to={`/Invoices/Invoice/${x.id}`}>{x.receivingCompany}</Link></td>
                                            <td>{ x.total.toFixed(2) }</td>
                                            <td>{ InvoiceStatus[x.status] }</td>
                                            <td>
                                                <a href={x.link} title="Bekijk pdf" target="_blank" rel="noreferrer"><i className="fa fa-file-pdf-o action-icon"></i></a>
                                                { x.status === InvoiceStatus.Sent && 
                                                    <button className="link" onClick={() => updateInvoiceStatus(x, InvoiceStatus.Paid)}><i className="fa fa-credit-card action-icon" title="Markeer als betaald"></i></button>
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
    );
}

export default Invoices;