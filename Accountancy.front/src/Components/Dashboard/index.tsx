import * as React from 'react';
import { Component, PropTypes } from 'react';
import { connect } from 'react-redux'

import Invoices from 'Components/Dashboard/Invoices';
import { Invoice } from './models';
import startup from './startup';

interface IProps {
    dispatch?: (action: any) => void;
    invoices: Invoice[]
}

var mapStateToProps = (state: any): IProps =>({
    invoices: state.invoices
});

class App extends Component<IProps> {
    render() {
        const props = this.props;
        return (
            <div>
                <h2>Le dashboard</h2>
                <Invoices invoices={props.invoices} />
            </div>
        )
    }

    componentWillMount() {
        startup(this.props.dispatch);
    }
}
export default connect(mapStateToProps)(App);