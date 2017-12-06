import Action from 'Infrastructure/Action';
import newState from 'Infrastructure/newState';
import * as actions from './Actions';
import { User } from './models';

class State {
    isFetching: boolean;
    isAuthenticated: boolean;
    user?: User;
}

const initialState: State = {
    isFetching: false,
    isAuthenticated: false
}

export default function reduce(oldState = initialState, action: Action<any>) {
    switch (action.type) {

        case actions.LOGIN_REQUEST:
            return newState(initialState, state => {
                state.isFetching = true;
                state.isAuthenticated = false;
            });

        case actions.LOGIN_SUCCESS:
            const data = action.payload as actions.LOGIN_SUCCESS;
            return newState(initialState, state => {
                state.isFetching = false;
                state.isAuthenticated = true;
                state.user = data.user;
            });
        
        case actions.LOGIN_FAILURE:
        case actions.LOGOUT_SUCCESS:
            return newState(initialState, state => {
                state.isFetching = false;
                state.isAuthenticated = false;
            });

        default:
            return oldState;

    }
}