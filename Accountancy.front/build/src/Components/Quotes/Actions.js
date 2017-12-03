"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const api_1 = require("Infrastructure/Middleware/api");
exports.QUOTE_REQUEST = 'QUOTE_REQUEST';
exports.QUOTE_SUCCESS = 'QUOTE_SUCCESS';
exports.QUOTE_FAILURE = 'QUOTE_FAILURE';
function fetchQuote() {
    return {
        [api_1.CALL_API]: {
            endpoint: 'random-quote',
            types: [exports.QUOTE_REQUEST, exports.QUOTE_SUCCESS, exports.QUOTE_FAILURE]
        }
    };
}
exports.fetchQuote = fetchQuote;
function fetchSecretQuote() {
    return {
        [api_1.CALL_API]: {
            endpoint: 'protected/random-quote',
            authenticated: true,
            types: [exports.QUOTE_REQUEST, exports.QUOTE_SUCCESS, exports.QUOTE_FAILURE]
        }
    };
}
exports.fetchSecretQuote = fetchSecretQuote;
