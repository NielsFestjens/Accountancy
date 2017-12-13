import Action from 'Infrastructure/Action';
import * as notifications from 'Components/Blocks/Notifications/Actions';
import * as DataService from './DataService';
import { User } from './models';

export const LOGIN_REQUEST = 'LOGIN_REQUEST';
export const requestLogin = (username: string, password: string): Action => ({ type: LOGIN_REQUEST })

export const LOGIN_SUCCESS = 'LOGIN_SUCCESS';
export type LOGIN_SUCCESS = { user: User };
export const receiveLogin = (user: User): Action<LOGIN_SUCCESS> => ({ type: LOGIN_SUCCESS, payload: { user: user } })

export const LOGIN_FAILURE = 'LOGIN_FAILURE';
export const loginError = (): Action => ({ type: LOGIN_FAILURE });

export const LOGOUT_REQUEST = 'LOGOUT_REQUEST';
export const requestLogout = (): Action => ({ type: LOGOUT_REQUEST })

export const LOGOUT_SUCCESS = 'LOGOUT_SUCCESS';
export type LOGOUT_SUCCESS = { isFetching: boolean, isAuthenticated: boolean };
export const receiveLogout = (): Action<LOGOUT_SUCCESS> => ({ type: LOGOUT_SUCCESS })

export function registerUser(username: string, password: string) {
    return (dispatch: (action: any) => void) => {
        dispatch(requestLogin(username, password));

        DataService
            .register(username, password)
            .then(result => {
                if (result.response.ok) {
                    DataService.getLoggedInUser().then(response => {
                        dispatch(receiveLogin(response.content.user));
                    })
                } else {
                    dispatch(loginError());
                    dispatch(notifications.addError("Could not register: " + result.content.error));
                }
            });
    };
}

export function loginUser(username: string, password: string) {
    return (dispatch: (action: any) => void) => {
        dispatch(requestLogin(username, password));

        DataService.login(username, password).then(result => {
            if (result.response.ok) {
                DataService.getLoggedInUser().then(response => {
                    dispatch(receiveLogin(response.content.user));
                })
            } else {
                dispatch(loginError());
                dispatch(notifications.addError("Could not log in: " + result.content.error));
            }
        });
    };
}

export function logoutUser() {
    return (dispatch: (action: any) => void) => {
        dispatch(requestLogout());
        DataService.logout().then(result => {
            dispatch(receiveLogout());
        })
    }
}