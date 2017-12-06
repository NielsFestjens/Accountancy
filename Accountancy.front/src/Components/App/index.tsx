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
import AppStartup from 'Services/AppStartup';

interface IAppStateProps {
    dispatch?: (action: any) => void;
    history?: History;

    isAuthenticated: boolean;
    username: string;
    errorMessage: string;
}

interface IAppDispatchProps { }

type IAppProps = IAppStateProps & IAppDispatchProps;

function mapStateToProps(state: any): IAppStateProps {

    const { auth } = state;
    const { isAuthenticated, username, errorMessage } = auth;

    return {
        isAuthenticated,
        username,
        errorMessage
    }
}

class App extends Component<IAppProps> {
    render() {
        const props = this.props;
        return (
            <div>
                <Navbar isAuthenticated={props.isAuthenticated} errorMessage={props.errorMessage} dispatch={props.dispatch} username={props.username} />
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
        AppStartup(this.props.dispatch, this.props.history);
    }
}
export default withRouter(connect(mapStateToProps)(App));