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
              .then(response => response.json().then(content => ({ content, response })))
              .catch(err => {
                  console.log("Error: ", err);
                  throw err;
              });
    }
    
    post(path: string, body: any = {}) {
        const config: RequestInit = {
            method: 'POST',
            credentials: "include",
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(body)
        }
    
        return fetch(this.baseUri + path, config)
        .catch(err => {
            console.log("Error: ", err);
            throw err;
        });
    }
}