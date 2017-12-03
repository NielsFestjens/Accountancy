"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react");
const react_1 = require("react");
const react_redux_1 = require("react-redux");
const Actions = require("Components/Quotes/Actions");
const Navbar_1 = require("Components/Layout/Navbar");
const Quotes_1 = require("Components/Quotes/Quotes");
const Notifications_1 = require("Components/Blocks/Notifications");
function mapStateToProps(state) {
    const { quotes, auth, notifications } = state;
    const { quote, authenticated } = quotes;
    const { isAuthenticated, errorMessage } = auth;
    return {
        notifications,
        quote,
        isSecretQuote: authenticated,
        isAuthenticated,
        errorMessage
    };
}
class App extends react_1.Component {
    render() {
        const { dispatch, quote, isAuthenticated, errorMessage, isSecretQuote } = this.props;
        return (React.createElement("div", null,
            React.createElement(Notifications_1.default, null),
            React.createElement(Navbar_1.default, { isAuthenticated: isAuthenticated, errorMessage: errorMessage, dispatch: dispatch }),
            React.createElement("div", { className: 'container' },
                React.createElement(Quotes_1.default, { onQuoteClick: () => dispatch(Actions.fetchQuote()), onSecretQuoteClick: () => dispatch(Actions.fetchSecretQuote()), isAuthenticated: isAuthenticated, quote: quote, isSecretQuote: isSecretQuote }))));
    }
}
exports.default = react_redux_1.connect(mapStateToProps)(App);
