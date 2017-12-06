import Action from 'Infrastructure/Action';
import newState from 'Infrastructure/newState';
import * as actions from './Actions';
import Notification from 'Components/Blocks/Notifications/Notification';

class State {
    notifications: Notification[];
}

const initialState: State = {
    notifications: []
}

export default function notifications(oldState = initialState, action: any) {
    switch (action.type) {
        case actions.ADD_NOTIFICATION:{
            const data = action.payload as actions.ADD_NOTIFICATION;
            return newState(oldState, state => {
                state.notifications = [...oldState.notifications, data.notification];
            });
        }
        case actions.REMOVE_NOTIFICATION:{
            const data = action.payload as actions.REMOVE_NOTIFICATION;
            return newState(oldState, state => {
                state.notifications =oldState.notifications.filter(x => x.id !== action.payload.notification.id);
            });
        }
        default:
            return oldState;
    }
}