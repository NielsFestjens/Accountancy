"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const DataService = require("Services/DataService");
exports.LOGIN_REQUEST = 'LOGIN_REQUEST';
function requestLogin(username, password) {
    return {
        type: exports.LOGIN_REQUEST,
        payload: {
            isFetching: true,
            isAuthenticated: false,
            username,
            password
        }
    };
}
exports.requestLogin = requestLogin;
exports.LOGIN_SUCCESS = 'LOGIN_SUCCESS';
function receiveLogin(user) {
    return {
        type: exports.LOGIN_SUCCESS,
        payload: {
            isFetching: false,
            isAuthenticated: true,
            id_token: user.id_token
        }
    };
}
exports.receiveLogin = receiveLogin;
exports.LOGIN_FAILURE = 'LOGIN_FAILURE';
function loginError(message) {
    return {
        type: exports.LOGIN_FAILURE,
        payload: {
            isFetching: false,
            isAuthenticated: false,
            message
        }
    };
}
exports.loginError = loginError;
exports.LOGOUT_REQUEST = 'LOGOUT_REQUEST';
function requestLogout() {
    return {
        type: exports.LOGOUT_REQUEST,
        payload: {
            isFetching: true,
            isAuthenticated: true
        }
    };
}
exports.requestLogout = requestLogout;
exports.LOGOUT_SUCCESS = 'LOGOUT_SUCCESS';
function receiveLogout() {
    return {
        type: exports.LOGOUT_SUCCESS,
        payload: {
            isFetching: false,
            isAuthenticated: false
        }
    };
}
exports.receiveLogout = receiveLogout;
function registerUser(username, password) {
    return (dispatch) => {
        dispatch(requestLogin(username, password));
        DataService
            .register(username, password)
            .then(response => {
            debugger;
            if (response.ok) {
                DataService.getLoggedInUser().then(response => {
                    dispatch(receiveLogin(response.content));
                });
            }
            else {
                dispatch(loginError("Could not register"));
            }
        });
    };
}
exports.registerUser = registerUser;
function loginUser(username, password) {
    return (dispatch) => {
        dispatch(requestLogin(username, password));
        DataService.login(username, password).then(response => {
            if (response.ok) {
                DataService.getLoggedInUser().then(response => {
                    dispatch(receiveLogin(response.content));
                });
            }
            else {
                dispatch(loginError("Could not log in"));
            }
        });
    };
}
exports.loginUser = loginUser;
function logoutUser() {
    return (dispatch) => {
        dispatch(requestLogout());
        localStorage.removeItem('id_token');
        localStorage.removeItem('access_token');
        dispatch(receiveLogout());
    };
}
exports.logoutUser = logoutUser;
