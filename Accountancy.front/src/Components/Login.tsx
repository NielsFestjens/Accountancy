import * as React from 'react'
import { Component, PropTypes } from 'react'

export interface ILoginProps {
    dispatch?: (action: any) => void;
    errorMessage: string;
    onLoginClick: (username: string, password: string) => void;
    onRegisterClick: (username: string, password: string) => void;
}

export default class Login extends Component<ILoginProps> {

  render() {
    const { errorMessage } = this.props

    return (
      <div>
        <input type='text' ref='username' className="form-control" placeholder='Username'/>
        <input type='password' ref='password' className="form-control" placeholder='Password'/>
        <button onClick={(event) => this.handleLoginClick(event)} className="btn btn-primary">Login</button>
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