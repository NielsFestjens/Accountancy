"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react");
const react_1 = require("react");
class Quotes extends react_1.Component {
    render() {
        const { onQuoteClick, onSecretQuoteClick, isAuthenticated, quote, isSecretQuote } = this.props;
        return (React.createElement("div", null,
            React.createElement("div", { className: 'col-sm-3' },
                React.createElement("button", { onClick: onQuoteClick, className: "btn btn-primary" }, "Get Quote")),
            isAuthenticated &&
                React.createElement("div", { className: 'col-sm-3' },
                    React.createElement("button", { onClick: onSecretQuoteClick, className: "btn btn-warning" }, "Get Secret Quote")),
            React.createElement("div", { className: 'col-sm-6' },
                quote && !isSecretQuote &&
                    React.createElement("div", null,
                        React.createElement("blockquote", null, quote)),
                quote && isAuthenticated && isSecretQuote &&
                    React.createElement("div", null,
                        React.createElement("span", { className: "label label-danger" }, "Secret Quote"),
                        React.createElement("hr", null),
                        React.createElement("blockquote", null, quote)))));
    }
}
exports.default = Quotes;
