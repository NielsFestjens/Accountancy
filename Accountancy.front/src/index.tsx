import * as React from "react";
import * as ReactDOM from "react-dom";
import { Provider } from 'react-redux';
import * as Redux from 'redux';
import thunk from 'redux-thunk';
import { BrowserRouter } from "react-router-dom";

import 'bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css';

import App from "Components/App";
import reducers from "./Reducers";
import api from 'Infrastructure/Middleware/api';
import 'Styles/default.css';

let createStoreWithMiddleware = Redux.applyMiddleware(api, thunk)(Redux.createStore);
let store = createStoreWithMiddleware(reducers);

ReactDOM.render(
    <Provider store={store}>
        <BrowserRouter>
            <App />
        </BrowserRouter>
    </Provider>,
    document.getElementById("root")
);