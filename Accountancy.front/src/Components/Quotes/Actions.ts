import { Action } from 'Infrastructure/Action';
import { CALL_API } from 'Infrastructure/Middleware/api';

export const QUOTE_REQUEST = 'QUOTE_REQUEST';
export const QUOTE_SUCCESS = 'QUOTE_SUCCESS';
export const QUOTE_FAILURE = 'QUOTE_FAILURE';

export function fetchQuote() {
  return {
    [CALL_API]: {
      endpoint: 'random-quote',
      types: [QUOTE_REQUEST, QUOTE_SUCCESS, QUOTE_FAILURE]
    }
  }
}

export function fetchSecretQuote() {
  return {
    [CALL_API]: {
      endpoint: 'protected/random-quote',
      authenticated: true,
      types: [QUOTE_REQUEST, QUOTE_SUCCESS, QUOTE_FAILURE]
    }
  }
}
