import * as React from 'react'
import { Component, PropTypes } from 'react'
import { connect } from 'react-redux'
import { bindActionCreators, Dispatch } from "redux";
import { Route, Switch, withRouter } from 'react-router';
import { History } from 'history';

import Login from 'Components/Auth/Login';
import Navbar from 'Components/Layout/Navbar';
import NotificationsContainer from 'Components/Blocks/Notifications';
import Notification from 'Components/Blocks/Notifications/Notification';
import Dashboard from 'Components/Dashboard';
import startup from './startup';
import { User } from 'Components/Auth/models';

interface IProps {
    dispatch?: (action: any) => void;
    history?: History;

    isAuthenticated: boolean;
    user: User;
    errorMessage: string;
}

var mapStateToProps = (state: any): IProps => ({
    isAuthenticated: state.auth.isAuthenticated,
    user: state.auth.user,
    errorMessage: state.auth.errorMessage
})

class App extends Component<IProps> {
    render() {
        const props = this.props;
        return (
            <div>
                <Navbar isAuthenticated={props.isAuthenticated} errorMessage={props.errorMessage} dispatch={props.dispatch} user={props.user} />
                <div className='container'>
                    <Switch>
                        <Route path="/dashboard" component={Dashboard} />
                    </Switch>
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