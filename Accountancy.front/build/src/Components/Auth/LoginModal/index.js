"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react");
const react_1 = require("react");
class Login extends react_1.Component {
    render() {
        const { errorMessage } = this.props;
        return (React.createElement("div", null,
            React.createElement("input", { type: 'text', ref: 'username', className: "form-control", placeholder: 'Username' }),
            React.createElement("br", null),
            React.createElement("input", { type: 'password', ref: 'password', className: "form-control", placeholder: 'Password' }),
            React.createElement("br", null),
            React.createElement("button", { onClick: (event) => this.handleLoginClick(event), className: "btn btn-primary" }, "Login"),
            React.createElement("button", { onClick: (event) => this.handleRegisterClick(event), className: "btn btn-primary" }, "Register"),
            errorMessage &&
                React.createElement("p", null, errorMessage)));
    }
    handleLoginClick(event) {
        const username = this.refs.username;
        const password = this.refs.password;
        this.props.onLoginClick(username.value.trim(), password.value.trim());
    }
    handleRegisterClick(event) {
        const username = this.refs.username;
        const password = this.refs.password;
        this.props.onRegisterClick(username.value.trim(), password.value.trim());
    }
}
exports.default = Login;
