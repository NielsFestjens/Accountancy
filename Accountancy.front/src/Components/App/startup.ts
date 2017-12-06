import { getLoggedInUser } from "Components/Auth/DataService";
import { receiveLogin } from "Components/Auth/Actions";
import { History } from 'history';

export default (dispatch: (action: any) => void, history: History) => {
    getLoggedInUser().then(response => {
        if (response.content.user) {
            response.content.user && dispatch(receiveLogin(response.content.user));
            history.push('/dashboard');
        }
    });
}