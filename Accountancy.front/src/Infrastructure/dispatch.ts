import Action from "Infrastructure/Action";

let dispatch: (action: Action<any>) => void;

export const setDispatch = (newDipatch: (action: Action<any>) => void) => {
    dispatch = newDipatch;
}

export default (action: Action<any>) => {
    if (!dispatch) {
        console.error('Dispatcher has not been initialised!');
        return;
    }

    dispatch(action);
}