import * as actions from './Actions';

const initialQuotesState = {
  isFetching: false,
  quote: '',
  authenticated: false
};

export default function quotes(state = initialQuotesState, action: any) {
  switch (action.type) {

    case actions.QUOTE_REQUEST:
      return Object.assign({}, state, {
        isFetching: true
      });

    case actions.QUOTE_SUCCESS:

      return Object.assign({}, state, {
        isFetching: false,
        quote: action.response,
        authenticated: action.authenticated || false
      });

    case actions.QUOTE_FAILURE:

      return Object.assign({}, state, {
        isFetching: false
      });

    default:
      return state;
      
    }
}
