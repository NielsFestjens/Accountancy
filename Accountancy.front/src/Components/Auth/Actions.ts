import { Action } from 'Infrastructure/Action';
import * as DataService from 'Services/DataService';
import * as notifications from 'Components/Blocks/Notifications/Actions';

export const LOGIN_REQUEST = 'LOGIN_REQUEST';
export type LOGIN_REQUEST = { isFetching: boolean, isAuthenticated: boolean, username: string, password: string };
export function requestLogin(username: string, password: string): Action<LOGIN_REQUEST> {
  return {
    type: LOGIN_REQUEST,
    payload: {
      isFetching: true,
      isAuthenticated: false,
      username,
      password
    }
  }
}

export const LOGIN_SUCCESS = 'LOGIN_SUCCESS';
export type LOGIN_SUCCESS = { isFetching: boolean, isAuthenticated: boolean, username: string };
export function receiveLogin(user: any): Action<LOGIN_SUCCESS> {
  return {
    type: LOGIN_SUCCESS,
    payload: {
      isFetching: false,
      isAuthenticated: true,
      username: user.username
    }
  }
}

export const LOGIN_FAILURE = 'LOGIN_FAILURE';
export type LOGIN_FAILURE = { isFetching: boolean, isAuthenticated: boolean };
export function loginError(): Action<LOGIN_FAILURE> {
  return {
    type: LOGIN_FAILURE,
    payload: {
      isFetching: false,
      isAuthenticated: false
    }
  }
}

export const LOGOUT_REQUEST = 'LOGOUT_REQUEST';
export type LOGOUT_REQUEST = { isFetching: boolean, isAuthenticated: boolean };
export function requestLogout(): Action<LOGOUT_REQUEST> {
  return {
    type: LOGOUT_REQUEST,
    payload: {
      isFetching: true,
      isAuthenticated: true
    }
  };
}

export const LOGOUT_SUCCESS = 'LOGOUT_SUCCESS';
export type LOGOUT_SUCCESS = { isFetching: boolean, isAuthenticated: boolean };
export function receiveLogout(): Action<LOGOUT_SUCCESS> {
  return {
    type: LOGOUT_SUCCESS,
    payload: {
      isFetching: false,
      isAuthenticated: false
    }
  };
}

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
    localStorage.removeItem('id_token');
    localStorage.removeItem('access_token');
    dispatch(receiveLogout());
  }
}