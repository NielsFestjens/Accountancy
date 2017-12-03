"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const BASE_URL = 'http://localhost:3001/api/';
function callApi(endpoint, authenticated) {
    let token = localStorage.getItem('access_token') || null;
    let config = {};
    if (authenticated) {
        if (token) {
            config = {
                headers: { 'Authorization': `Bearer ${token}` }
            };
        }
        else {
            throw "No token saved!";
        }
    }
    return fetch(BASE_URL + endpoint, config)
        .then(response => response.text().then(text => ({ text, response }))).then(value => {
        if (!value.response.ok) {
            return Promise.reject(value.text);
        }
        return value.text;
    }).catch((err) => console.log(err));
}
exports.CALL_API = Symbol('Call API');
exports.default = (store) => (next) => (action) => {
    const callAPI = action[exports.CALL_API];
    if (typeof callAPI === 'undefined') {
        return next(action);
    }
    let { endpoint, types, authenticated } = callAPI;
    const [requestType, successType, errorType] = types;
    return callApi(endpoint, authenticated).then((response) => next({
        response,
        authenticated,
        type: successType
    }), (error) => next({
        error: error.message || 'There was an error.',
        type: errorType
    }));
};
