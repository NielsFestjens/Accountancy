import * as React from 'react';
import { Component, PropTypes } from 'react';
import { Link } from 'react-router-dom';

import { InvoiceDto } from './models';
import * as Actions from './Actions';
import { InvoiceStatus } from 'Components/Invoices/models';

export interface IProps {
    dispatch: (action: any) => void;
    invoices: InvoiceDto[]
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
                            <th>Naam</th>
                            <th>Status</th>
                            <th>Acties</th>
                        </tr>
                    </thead>
                    <tbody>
                        {this.props.invoices.map(x => 
                            <tr key={x.id}>
                                <td><Link to={`/Invoices/Invoice/${x.id}`}>{x.name}</Link></td>
                                <td>{InvoiceStatus[x.status]}</td>
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
        )
    }
}