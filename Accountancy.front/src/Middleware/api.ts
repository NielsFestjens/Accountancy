
const BASE_URL = 'http://localhost:3001/api/'

function callApi(endpoint: string, authenticated: boolean) {

  let token = localStorage.getItem('access_token') || null
  let config = {}

  if(authenticated) {
    if(token) {
      config = {
        headers: { 'Authorization': `Bearer ${token}` }
      }
    }
    else {
      throw "No token saved!"
    }
  }

  return fetch(BASE_URL + endpoint, config)
    .then(response =>
      response.text().then(text => ({ text, response }))
    ).then(value => {
      if (!value.response.ok) {
        return Promise.reject(value.text) as any;
      }

      return value.text;
    }).catch((err: any) => console.log(err))
}

export const CALL_API = Symbol('Call API')

export default (store: any) => (next: any) => (action: any) => {

  const callAPI = action[CALL_API]

  if (typeof callAPI === 'undefined') {
    return next(action)
  }

  let { endpoint, types, authenticated } = callAPI

  const [ requestType, successType, errorType ] = types

  return callApi(endpoint, authenticated).then(
    (response: any) =>
      next({
        response,
        authenticated,
        type: successType
      }),
    (error: any) => next({
      error: error.message || 'There was an error.',
      type: errorType
    })
  )
}