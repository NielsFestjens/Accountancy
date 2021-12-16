import ApiCaller from 'Infrastructure/ApiCaller';
import { apiUri } from 'config';

let apiCaller = new ApiCaller(apiUri + 'Auth/');

export const register = (onError: (message: string) => void, username: string, password: string) => apiCaller.post(onError, 'register', { username, password });
export const login = (onError: (message: string) => void, username: string, password: string) => apiCaller.post(onError, 'login', { username, password });
export const getLoggedInUser = (onError: (message: string) => void,) => apiCaller.get(onError, 'getLoggedInUser');
export const logout = (onError: (message: string) => void,) => apiCaller.post(onError, 'logout');