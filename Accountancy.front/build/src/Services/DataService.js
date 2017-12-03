"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const ApiCaller_1 = require("./ApiCaller");
let apiCaller = new ApiCaller_1.default('http://localhost:8081/api/');
function register(username, password) {
    const body = {
        Username: username,
        Password: password
    };
    return apiCaller.post('register', body);
}
exports.register = register;
function login(username, password) {
    const body = {
        Username: username,
        Password: password
    };
    return apiCaller.post('login', body);
}
exports.login = login;
function getLoggedInUser() {
    return apiCaller.get('getLoggedInUser');
}
exports.getLoggedInUser = getLoggedInUser;
function logout() {
    return apiCaller.post('logout');
}
exports.logout = logout;
