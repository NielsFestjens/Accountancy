"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react");
const react_1 = require("react");
const react_redux_1 = require("react-redux");
function mapStateToProps(state) {
    const { notifications } = state;
    return {
        notifications: notifications.notifications
    };
}
class Notifications extends react_1.Component {
    render() {
        const { notifications } = this.props;
        return (React.createElement("div", { className: "notifications" }, notifications.map(x => React.createElement("span", null,
            "x.message",
            React.createElement("br", null)))));
    }
}
exports.default = react_redux_1.connect(mapStateToProps)(Notifications);
