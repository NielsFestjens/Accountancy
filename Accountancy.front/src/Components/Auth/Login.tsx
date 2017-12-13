import * as React from 'react'
import { Component, PropTypes } from 'react'

export interface ILoginProps {
    onLoginClick: (username: string, password: string) => void;
    onRegisterClick: (username: string, password: string) => void;
}

export default class Login extends Component<ILoginProps> {

  render() {
    return (
      <div>
        <input type='text' ref='username' className="form-control" style={{width:100, display:"inline"}} placeholder='Username'/>&nbsp;
        <input type='password' ref='password' className="form-control" style={{width:100, display:"inline"}} placeholder='Password'/>&nbsp;
        <button onClick={this.handleLoginClick} className="btn btn-primary">Login</button>&nbsp;
        <button onClick={this.handleRegisterClick} className="btn btn-primary">Register</button>
      </div>
    )
  }

  handleLoginClick = () => {
    const username = this.refs.username as HTMLInputElement;
    const password = this.refs.password as HTMLInputElement;
    this.props.onLoginClick(username.value.trim(), password.value.trim());
  }

  handleRegisterClick = () => {
    const username = this.refs.username as HTMLInputElement;
    const password = this.refs.password as HTMLInputElement;
    this.props.onRegisterClick(username.value.trim(), password.value.trim());
  }
}