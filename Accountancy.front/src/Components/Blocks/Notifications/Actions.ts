import Action from 'Infrastructure/Action';
import { Notification, NotificationType } from 'Components/Blocks/Notifications/Notification';
import uniqueId from 'Infrastructure/uniqueId';

export const ADD_NOTIFICATION = 'ADD_NOTIFICATION';
export type ADD_NOTIFICATION = { notification: Notification };
export const addError = (message: string): Action<ADD_NOTIFICATION> => ({
    type: ADD_NOTIFICATION,
    payload: {
        notification: {
            id: uniqueId(),
            message,
            type: NotificationType.error
        }
    }
})

export const REMOVE_NOTIFICATION = 'REMOVE_NOTIFICATION';
export type REMOVE_NOTIFICATION = { notification: Notification };
export const removeNotification = (notification: Notification): Action<REMOVE_NOTIFICATION> => ({
    type: REMOVE_NOTIFICATION,
    payload: {
        notification: notification
    }
})