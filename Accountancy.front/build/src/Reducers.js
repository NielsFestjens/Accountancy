"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const redux_1 = require("redux");
const Reducers_1 = require("Components/Auth/Reducers");
const Reducers_2 = require("Components/Quotes/Reducers");
const Reducers_3 = require("Components/Blocks/Notifications/Reducers");
const reducers = redux_1.combineReducers({
    auth: Reducers_1.default,
    quotes: Reducers_2.default,
    notifications: Reducers_3.default
});
exports.default = reducers;
