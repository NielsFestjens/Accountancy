import * as React from 'react'
import { Component } from 'react'
import { connect } from 'react-redux'
import { bindActionCreators, Dispatch } from "redux";
import { Route, Switch, withRouter } from 'react-router';
import { History } from 'history';

import Login from 'Components/Auth/Login';
import Navbar from 'Components/Layout/Navbar';
import NotificationsContainer from 'Components/Blocks/Notifications';
import Notification from 'Components/Blocks/Notifications/Notification';
import Dashboard from 'Components/Dashboard';
import Invoices from 'Components/Invoices';
import startup from './startup';
import { User } from 'Components/Auth/models';
import State from 'State';

interface IProps {
    dispatch?: (action: any) => void;
    history?: History;

    isAuthenticated: boolean;
    user: User;
}

var mapStateToProps = (state: State): IProps => ({
    isAuthenticated: state.auth.isAuthenticated,
    user: state.auth.user,
})

class App extends Component<IProps> {
    render() {
        const props = this.props;
        return (
            <div>
                <Navbar isAuthenticated={props.isAuthenticated}  dispatch={props.dispatch} user={props.user} />
                <div className='container'>
                    {props.isAuthenticated && 
                        <Switch>
                            <Route path="/dashboard" component={Dashboard} />
                            <Route path="/invoices" component={Invoices} />
                        </Switch>
                    }
                </div>
                <NotificationsContainer />
            </div>
        )
    }

    componentWillMount() {
        startup(this.props.dispatch, this.props.history);
    }
}

export default withRouter(connect(mapStateToProps)(App));