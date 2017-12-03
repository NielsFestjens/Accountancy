"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.ADD_NOTIFICATION = 'ADD_NOTIFICATION';
function addNotification(message) {
    return {
        type: exports.ADD_NOTIFICATION,
        payload: {
            message
        }
    };
}
exports.addNotification = addNotification;
