import * as React from 'react';
import { Component } from 'react';
import { connect } from 'react-redux'

import Invoices from 'Components/Dashboard/Invoices';
import { InvoiceDto, InvoiceYear } from './models';
import * as Actions from "./Actions";
import State from 'State';

interface IProps {
    dispatch?: (action: any) => void;
    invoices: InvoiceYear[]
}

var mapStateToProps = (state: State): IProps => ({
    invoices: state.dashboard.invoiceYears
});

class Dashboard extends Component<IProps> {
    render() {
        const props = this.props;
        return (
            <div>
                <h2>Le dashboard</h2>
                <Invoices invoices={props.invoices} dispatch={props.dispatch} />
            </div>
        )
    }

    componentWillMount() {
        this.props.dispatch(Actions.fetchInvoices());
    }
}
export default connect(mapStateToProps)(Dashboard);