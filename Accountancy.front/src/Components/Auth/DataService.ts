import ApiCaller from 'Infrastructure/ApiCaller';
import { apiUri } from 'config';

let apiCaller = new ApiCaller(apiUri);

export function register(username: string, password: string) {
    const body = {
        Username: username,
        Password: password
    }
    return apiCaller.post('register', body);
}

export function login(username: string, password: string) {
    const body = {
        Username: username,
        Password: password
    }
    return apiCaller.post('login', body);
}

export function getLoggedInUser() {
    return apiCaller.get('getLoggedInUser')
}

export function logout() {
    return apiCaller.post('logout');
}