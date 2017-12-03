"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react");
const ReactDOM = require("react-dom");
const react_redux_1 = require("react-redux");
const Redux = require("redux");
const redux_thunk_1 = require("redux-thunk");
require("bootstrap");
require("bootstrap/dist/css/bootstrap.css");
const App_1 = require("Components/App");
const Reducers_1 = require("./Reducers");
const api_1 = require("Infrastructure/Middleware/api");
require("Styles/default.css");
let createStoreWithMiddleware = Redux.applyMiddleware(api_1.default, redux_thunk_1.default)(Redux.createStore);
let store = createStoreWithMiddleware(Reducers_1.default);
ReactDOM.render(React.createElement(react_redux_1.Provider, { store: store },
    React.createElement(App_1.default, null)), document.getElementById("root"));
