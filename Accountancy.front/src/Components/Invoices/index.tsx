import * as React from 'react';
import { Component, PropTypes } from 'react';
import { connect } from 'react-redux'
import { Switch } from 'react-router-dom';
import { Route } from 'react-router';
import Detail from './Detail';
import State from 'State';

interface IProps {
    dispatch?: (action: any) => void;
}

var mapStateToProps = (state: State): IProps => ({
});

class Invoices extends Component<IProps> {
    render() {
        const props = this.props;
        return (
            <Switch>
                <Route path="/Invoices/Invoice/:id" component={Detail} />
            </Switch>
        )
    }
}
export default connect(mapStateToProps)(Invoices);