import { Action } from 'Actions/Action'

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

export function logoutUser() {
  return (dispatch: (action: any) => void) => {
    dispatch(requestLogout());
    localStorage.removeItem('id_token');
    localStorage.removeItem('access_token');
    dispatch(receiveLogout());
  }
}