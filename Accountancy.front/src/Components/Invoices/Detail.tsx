import * as React from 'react';
import { Component } from 'react';
import { Link } from 'react-router-dom';
import { fetchInvoice } from './Actions';
import { match } from 'react-router';
import { Invoice, InvoiceStatus } from 'Components/Invoices/models';
import { connect } from 'react-redux';

interface IParams {
    id: number;
}

export interface IProps {
    dispatch?: (action: any) => void;
    match?: match<IParams>;
    invoice?: Invoice
}

var mapStateToProps = (state: any): IProps => ({
    invoice: state.invoices.invoice
});

class Detail extends Component<IProps> {
    render() {
        const props = this.props;

        return (
            <div>
                <h2>Factuur detail</h2>
                { props.invoice && 
                    <div>
                        Jaar: { props.invoice.year }<br />
                        Maand: { props.invoice.month }<br />
                        Status: { InvoiceStatus[props.invoice.status] }<br />
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
                            {this.props.invoice.invoiceLines.map(x => 
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
        )
    }

    componentDidMount() {
        this.props.dispatch(fetchInvoice(this.props.match.params.id));
    }
}

export default connect(mapStateToProps)(Detail);