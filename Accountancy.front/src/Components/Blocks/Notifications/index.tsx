import * as React from 'react'
import { Component } from 'react'
import { connect } from 'react-redux'

import Notification from 'Components/Blocks/Notifications/Notification';
import * as actions from 'Components/Blocks/Notifications/Actions';
import State from 'State';

interface INotificationsContainerStateProps {
    dispatch?: (action: any) => void;
    notifications: Notification[]
}

interface INotificationsContainerDispatchProps { 

}

type INotificationsContainerProps = INotificationsContainerStateProps & INotificationsContainerDispatchProps;

function mapStateToProps(state: State): INotificationsContainerStateProps {

    const { notifications } = state;
    return {
        notifications: notifications.notifications
    }
}

class NotificationsContainer extends Component<INotificationsContainerProps> {
    render() {
        const { notifications } = this.props
        return (
            <div className="notifications">
                {notifications.map(x => <div key={x.id} className="notification" onClick={() => this.handleNotificationClick(x)}>{x.message}<br /></div>)}
            </div>
        )
    }

    handleNotificationClick(notification: Notification) {
        this.props.dispatch(actions.removeNotification(notification));
    }
}
export default connect(mapStateToProps)(NotificationsContainer);