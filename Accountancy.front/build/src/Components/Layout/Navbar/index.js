"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react");
const react_1 = require("react");
const Login_1 = require("Components/Auth/Login");
const Logout_1 = require("Components/Auth/Logout");
const Actions_1 = require("Components/Auth/Actions");
class Navbar extends react_1.Component {
    render() {
        const { dispatch, isAuthenticated, errorMessage } = this.props;
        return (React.createElement("nav", { className: "navbar navbar" },
            React.createElement("a", { className: "navbar-brand", href: "#" }, "Accountancy"),
            React.createElement("div", { className: 'navbar-form' },
                !isAuthenticated &&
                    React.createElement(Login_1.default, { errorMessage: errorMessage, onLoginClick: (username, password) => dispatch(Actions_1.loginUser(username, password)), onRegisterClick: (username, password) => dispatch(Actions_1.registerUser(username, password)) }),
                isAuthenticated &&
                    React.createElement(Logout_1.default, { onLogoutClick: () => dispatch(Actions_1.logoutUser()) }))));
    }
}
exports.default = Navbar;
