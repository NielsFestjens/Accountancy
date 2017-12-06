import * as notifications from 'Components/Blocks/Notifications/Actions'

export default class ApiCaller {
    constructor(private baseUri: string) {
    }

    private convertToQueryString(request: any) {
        const parts: string[] = [];
        for (var key in request) {
            parts.push(`${encodeURIComponent(key)}=${encodeURIComponent(request[key])}`);
        }
        return parts.join('&');
    }
    
    get(path: string, request: any = {}) {
        const config: RequestInit = {
            method: 'GET',
            credentials: 'include'
        }
        return fetch(`${this.baseUri}${path}?${this.convertToQueryString(request)}`, config)
              .then(response => !response.ok ? { response, content: undefined } : response.json().then(content => ({ response, content })))
              .catch(error => {
                  notifications.addError(error.message);
                  console.log("Error: ", error);
                  throw error;
              });
    }
    
    post(path: string, body: any = {}) {
        const config: RequestInit = {
            method: 'POST',
            credentials: "include",
            headers: [
                ['Content-Type', 'application/json']
            ],
            body: JSON.stringify(body)
        }
    
        return fetch(this.baseUri + path, config)
            .then(response => response.ok ? {response, content: undefined } : response.json().then(content => ({ response, content })))
            .catch(error => {
                notifications.addError(error.message);
                console.log("Error: ", error);
                throw error;
            });
    }
}