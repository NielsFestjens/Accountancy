import * as React from 'react'
import { Component } from 'react'
import { connect } from 'react-redux'

import Notification from 'Components/Blocks/Notifications/Notification';
import * as actions from 'Components/Blocks/Notifications/Actions';
import State from 'State';
import { Action } from 'redux';

interface IProps {
    dispatch?: (action: any) => void;
    
    notifications: Notification[]
}

const mapStateToProps = (state: State): IProps => ({
    notifications: state.notifications.notifications
})

class NotificationsContainer extends Component<IProps> {
    render() {
        const { notifications } = this.props
        return (
            <div className="notifications">
                {notifications.map(x => <div key={x.id} className="alert alert-danger" onClick={() => this.handleNotificationClick(x)}>{x.message}<br /></div>)}
            </div>
        )
    }

    handleNotificationClick(notification: Notification) {
        this.props.dispatch(actions.removeNotification(notification));
    }
}
export default connect(mapStateToProps)(NotificationsContainer);