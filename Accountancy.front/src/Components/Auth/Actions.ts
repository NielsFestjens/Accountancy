import { Action } from 'Infrastructure/Action';
import * as DataService from 'Services/DataService';

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
export type LOGIN_SUCCESS = { isFetching: boolean, isAuthenticated: boolean, id_token: string };
export function receiveLogin(user: any): Action<LOGIN_SUCCESS> {
  return {
    type: LOGIN_SUCCESS,
    payload: {
      isFetching: false,
      isAuthenticated: true,
      id_token: user.id_token
    }
  }
}

export const LOGIN_FAILURE = 'LOGIN_FAILURE';
export type LOGIN_FAILURE = { isFetching: boolean, isAuthenticated: boolean, message: string };
export function loginError(message: any): Action<LOGIN_FAILURE> {
  return {
    type: LOGIN_FAILURE,
    payload: {
      isFetching: false,
      isAuthenticated: false,
      message
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
    
    DataService.register(username, password).then(response => {
        if (response.ok) {
          DataService.getLoggedInUser().then(({ content, response }) => {
            dispatch(receiveLogin(content));
          })
        } else {
          dispatch(loginError("Could not register"));
          return Promise.reject("Could not register");
        }
      });
  };
}

export function loginUser(username: string, password: string) {

  return (dispatch: (action: any) => void) => {
    dispatch(requestLogin(username, password));
    
    DataService.login(username, password).then(response => {
        if (response.ok) {
          DataService.getLoggedInUser().then(({ content, response }) => {
            dispatch(receiveLogin(content));
          })
        } else {
          dispatch(loginError("Could not log in"));
          return Promise.reject("Could not log in");
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