import { getLoggedInUser } from "Components/Auth/DataService";
import { receiveLogin } from "Components/Auth/Actions";
import { History } from 'history';
import Action from "Infrastructure/Action";
import { setDispatch } from 'Infrastructure/dispatch';

export default (dispatch: (action: Action) => void, history: History) => {
    setDispatch(dispatch);

    getLoggedInUser().then(response => {
        if (response.content.user) {
            response.content.user && dispatch(receiveLogin(response.content.user));
            if (history.location.pathname === '/')
                history.push('/dashboard');
        }
    });
}