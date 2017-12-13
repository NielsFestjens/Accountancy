import ApiCaller from 'Infrastructure/ApiCaller';
import { apiUri } from 'config';

let apiCaller = new ApiCaller(apiUri + 'Auth/');

export const register = (username: string, password: string) => apiCaller.post('register', { username, password });
export const login = (username: string, password: string) => apiCaller.post('login', { username, password });
export const getLoggedInUser = () => apiCaller.get('getLoggedInUser');
export const logout = () => apiCaller.post('logout');