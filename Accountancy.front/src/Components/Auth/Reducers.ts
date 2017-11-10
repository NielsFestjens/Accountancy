import * as actions from './Actions';

const initialAuthState = {
  isFetching: false,
  isAuthenticated: localStorage.getItem('id_token') ? true : false
}
 
export default function auth(state = initialAuthState, action: any) {
  switch (action.type) {

    case actions.LOGIN_REQUEST:
      return Object.assign({}, state, {
        isFetching: true,
        isAuthenticated: false,
        user: action.creds
      });

    case actions.LOGIN_SUCCESS:
      return Object.assign({}, state, {
        isFetching: false,
        isAuthenticated: true,
        errorMessage: ''
      });

    case actions.LOGIN_FAILURE:
      return Object.assign({}, state, {
        isFetching: false,
        isAuthenticated: false,
        errorMessage: action.message
      });
      
      case actions.LOGOUT_SUCCESS:
        return Object.assign({}, state, {
          isFetching: true,
          isAuthenticated: false
        });

    default:
      return state;

  }
}