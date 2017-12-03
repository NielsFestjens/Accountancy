"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const actions = require("./Actions");
const initialAuthState = {
    isFetching: false,
    isAuthenticated: localStorage.getItem('id_token') ? true : false
};
function auth(state = initialAuthState, action) {
    switch (action.type) {
        case actions.LOGIN_REQUEST:
            return Object.assign({}, state, {
                isFetching: true,
                isAuthenticated: false,
                user: action.creds
            });
        case actions.LOGIN_SUCCESS:
            return Object.assign({}, state, {
                isFetching: false,
                isAuthenticated: true,
                errorMessage: ''
            });
        case actions.LOGIN_FAILURE:
            return Object.assign({}, state, {
                isFetching: false,
                isAuthenticated: false,
                errorMessage: action.message
            });
        case actions.LOGOUT_SUCCESS:
            return Object.assign({}, state, {
                isFetching: true,
                isAuthenticated: false
            });
        default:
            return state;
    }
}
exports.default = auth;
