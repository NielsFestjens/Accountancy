import * as React from 'react';
import { Component, PropTypes } from 'react';
import { connect } from 'react-redux'

import Invoices from 'Components/Dashboard/Invoices';
import { InvoiceDto } from './models';
import * as Actions from "./Actions";
import State from 'State';

interface IProps {
    dispatch?: (action: any) => void;
    invoices: InvoiceDto[]
}

var mapStateToProps = (state: State): IProps => {
    return  { 
        invoices: state.dashboard.invoices
    };
};

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