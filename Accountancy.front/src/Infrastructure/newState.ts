export default function newState <T>(oldstate: T, modifyFunc: (newState: T) => void) {
    const theNewstate = Object.assign({}, oldstate);
    modifyFunc(theNewstate);
    return theNewstate;
}