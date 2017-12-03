"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react");
const react_1 = require("react");
class Logout extends react_1.Component {
    render() {
        const { onLogoutClick } = this.props;
        return (React.createElement("button", { onClick: () => onLogoutClick(), className: "btn btn-primary" }, "Logout"));
    }
}
exports.default = Logout;
