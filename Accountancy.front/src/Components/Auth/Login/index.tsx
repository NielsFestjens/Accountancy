import * as React from 'react'
import { Component, PropTypes } from 'react'

export interface ILoginProps {
    errorMessage: string;
    onLoginClick: (username: string, password: string) => void;
    onRegisterClick: (username: string, password: string) => void;
}

export default class Login extends Component<ILoginProps> {

  render() {
    const { errorMessage } = this.props

    return (
      <div>
        <input type='text' ref='username' className="form-control" style={{width:100, display:"inline"}} placeholder='Username'/>&nbsp;
        <input type='password' ref='password' className="form-control" style={{width:100, display:"inline"}} placeholder='Password'/>&nbsp;
        <button onClick={(event) => this.handleLoginClick(event)} className="btn btn-primary">Login</button>&nbsp;
        <button onClick={(event) => this.handleRegisterClick(event)} className="btn btn-primary">Register</button>

        {errorMessage &&
          <p>{errorMessage}</p>
        }
      </div>
    )
  }

  handleLoginClick(event: React.MouseEvent<HTMLButtonElement>) {
    const username = this.refs.username as HTMLInputElement;
    const password = this.refs.password as HTMLInputElement;
    this.props.onLoginClick(username.value.trim(), password.value.trim());
  }

  handleRegisterClick(event: React.MouseEvent<HTMLButtonElement>) {
    const username = this.refs.username as HTMLInputElement;
    const password = this.refs.password as HTMLInputElement;
    this.props.onRegisterClick(username.value.trim(), password.value.trim());
  }
}