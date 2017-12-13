import * as notifications from 'Components/Blocks/Notifications/Actions'
import dispatch from 'Infrastructure/dispatch';

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
                dispatch(notifications.addError(error.message));
                  console.error(error);
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
            .then(this.handlePostResponse)
            .catch(error => {
                dispatch(notifications.addError(error.message));
                console.error( error);
                throw error;
            });
    }

    private handlePostResponse(response: Response) {
        if (response.ok)
            return { response, content: undefined as any };

        if (response.status === 500)
            return response.json().then(content => ({ response, content }));
        
        dispatch(notifications.addError('Er is iets misgegaan tijdens het uitvoeren van een opdracht op de server'));
        console.error(response);
    }
}