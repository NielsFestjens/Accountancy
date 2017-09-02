import * as React from 'react'
import { Component, PropTypes } from 'react'
import Login from './Login'
import Logout from './Logout'
import { loginUser, logoutUser, registerUser } from 'actions'

export interface INavbarProps {
    dispatch?: (action: any) => void;
    isAuthenticated: boolean;
    errorMessage: string 
}

export default class Navbar extends Component<INavbarProps> {

  render() {
    const { dispatch, isAuthenticated, errorMessage } = this.props

    return (
      <nav className='navbar navbar-default'>
        <div className='container-fluid'>
          <a className="navbar-brand" href="#">Quotes App</a>
          <div className='navbar-form'>

            {!isAuthenticated &&
              <Login
                errorMessage={errorMessage}
                onLoginClick={ (username: string, password: string) => dispatch(loginUser(username, password)) }
                onRegisterClick={ (username: string, password: string) => dispatch(registerUser(username, password)) }
              />
            }

            {isAuthenticated &&
              <Logout onLogoutClick={() => dispatch(logoutUser())} />
            }

          </div>
        </div>
      </nav>
    )
  }

}