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
    
    get(onError: (message: string) => void, path: string, request: any = {}) {
        const config: RequestInit = {
            method: 'GET',
            credentials: 'include'
        }
        return fetch(`${this.baseUri}${path}?${this.convertToQueryString(request)}`, config)
            .then(response => !response.ok ? { response, content: undefined } : response.json().then(content => ({ response, content })))
            .catch(error => {
                onError(error.message);
                console.error(error);
                throw error;
            });
    }
    
    post(onError: (message: string) => void, path: string, body: any = {}) {
        const config: RequestInit = {
            method: 'POST',
            credentials: "include",
            headers: [
                ['Content-Type', 'application/json']
            ],
            body: JSON.stringify(body)
        }
    
        return fetch(this.baseUri + path, config)
            .then(x => this.handlePostResponse(onError, x))
            .catch(error => {
                onError(error.message);
                console.error( error);
                throw error;
            });
    }

    private handlePostResponse(onError: (message: string) => void, response: Response) {
        if (response.ok)
            return { response, content: undefined as any };

        if (response.status === 400 || response.status === 500)
            return response.json().then(content => ({ response, content }));
        
        onError('Er is iets misgegaan tijdens het uitvoeren van een opdracht op de server');
        console.error(response);
    }
}