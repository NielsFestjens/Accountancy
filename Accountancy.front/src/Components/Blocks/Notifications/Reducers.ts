import * as actions from './Actions';
import { Action } from 'Infrastructure/Action';
import Notification from 'Components/Blocks/Notifications/Notification';

const emptyNotifications: Notification[] = [];
const initialState = {
    notifications: emptyNotifications
}

export default function notifications(state = initialState, action: any) {
    switch (action.type) {

        case actions.ADD_NOTIFICATION:
            return Object.assign({}, state, {
                notifications: [...state.notifications, action.payload.notification]
            });

        case actions.REMOVE_NOTIFICATION:
            return Object.assign({}, state, {
                notifications: state.notifications.filter(x => x.id !== action.payload.notification.id)
            });

        default:
            return state;

    }
}