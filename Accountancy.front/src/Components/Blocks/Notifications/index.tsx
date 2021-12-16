import React from 'react'
import Notification from './Notification';

interface IProps {
    notifications: Notification[];
    setNotifications: (notifications: Notification[]) => void;
}

const NotificationsContainer = (props: IProps) => {

    const { notifications, setNotifications } = props;

    const handleNotificationClick = (notification: Notification) => {
        setNotifications(notifications.filter(x => x.id !== notification.id));
    }

    return (
        <div className="notifications">
            {notifications.map(x => <div key={x.id} className="alert alert-danger" onClick={() => handleNotificationClick(x)}>{x.message}<br /></div>)}
        </div>
    )
}

export default NotificationsContainer;