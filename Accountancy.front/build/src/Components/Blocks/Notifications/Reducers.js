"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const actions = require("./Actions");
const emptyNotifications = [];
const initialState = {
    notifications: emptyNotifications
};
function notifications(state = initialState, action) {
    switch (action.type) {
        case actions.ADD_NOTIFICATION:
            return Object.assign({}, state, {
                notifications: [...state.notifications, action.Notification]
            });
        default:
            return state;
    }
}
exports.default = notifications;
