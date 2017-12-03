"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const notifications = require("Components/Blocks/Notifications/Actions");
class ApiCaller {
    constructor(baseUri) {
        this.baseUri = baseUri;
    }
    convertToQueryString(request) {
        const parts = [];
        for (var key in request) {
            parts.push(`${encodeURIComponent(key)}=${encodeURIComponent(request[key])}`);
        }
        return parts.join('&');
    }
    get(path, request = {}) {
        const config = {
            method: 'GET',
            credentials: 'include'
        };
        return fetch(`${this.baseUri}${path}?${this.convertToQueryString(request)}`, config)
            .then(response => response.json().then(content => (Object.assign({}, response, { content }))))
            .catch(error => {
            notifications.addNotification(error.message);
            console.log("Error: ", error);
            throw error;
        });
    }
    post(path, body = {}) {
        const config = {
            method: 'POST',
            credentials: "include",
            headers: [
                ['Content-Type', 'application/json']
            ],
            body: JSON.stringify(body)
        };
        return fetch(this.baseUri + path, config)
            .then(response => response.json().then(content => (Object.assign({}, response, { content }))))
            .catch(error => {
            notifications.addNotification(error.message);
            console.log("Error: ", error);
            throw error;
        });
    }
}
exports.default = ApiCaller;
