import { Action } from 'Infrastructure/Action';
import * as DataService from 'Services/DataService';
import { Notification, NotificationType} from 'Components/Blocks/Notifications/Notification';
import uniqueId from 'Services/uniqueId';

export const ADD_NOTIFICATION = 'ADD_NOTIFICATION';
export type ADD_NOTIFICATION = { notification: Notification };
export function addError(message: string): Action<ADD_NOTIFICATION> {
  return {
    type: ADD_NOTIFICATION,
    payload: {
      notification: {
        id: uniqueId(),
        message,
        type: NotificationType.error
      }
    }
  }
}

export const REMOVE_NOTIFICATION = 'REMOVE_NOTIFICATION';
export type REMOVE_NOTIFICATION = { notification: Notification };
export function removeNotification(notification: Notification): Action<REMOVE_NOTIFICATION> {
  return {
    type: REMOVE_NOTIFICATION,
    payload: {
      notification: notification
    }
  }
}