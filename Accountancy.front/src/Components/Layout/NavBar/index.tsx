import * as React from 'react';
import { Component } from 'react';
import { Link } from 'react-router-dom';
import { History } from 'history';

import Login from 'Components/Auth/Login';
import Logout from 'Components/Auth/Logout';
import { loginUser, registerUser, logoutUser } from 'Components/Auth/Actions';
import { User } from 'Components/Auth/models';

export interface IProps {
    dispatch?: (action: any) => void;
    isAuthenticated: boolean;
    user: User;
    history: History
}

export default class Navbar extends Component<IProps> {

    render() {
        const props = this.props

        return (
            <nav className="navbar navbar-expand-lg navbar-light bg-light">
                <a className="navbar-brand" href="#">Accountancy</a>
                
                <button className="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                    <span className="navbar-toggler-icon"></span>
                </button>
                
                <div className="collapse navbar-collapse" id="navbarSupportedContent">

                    {!props.isAuthenticated &&
                        <Login
                            onLoginClick={(username: string, password: string) => props.dispatch(loginUser(username, password, props.history))}
                            onRegisterClick={(username: string, password: string) => props.dispatch(registerUser(username, password, props.history))}
                        />
                    }

                    {props.isAuthenticated &&
                        <ul className="navbar-nav mr-auto">
                            <li className="nav-item">
                                <Link to="/dashboard" className="nav-link">Dashboard</Link>
                            </li>
                            <li className="nav-item dropdown">
                                <a className="nav-link dropdown-toggle" href="#" id="navbarDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                    {props.user.username}
                                </a>
                                <div className="dropdown-menu" aria-labelledby="navbarDropdown">
                                    <a className="dropdown-item" href="#" onClick={() => props.dispatch(logoutUser())}>Logout</a>
                                </div>
                            </li>
                        </ul>
                    }

                    {props.isAuthenticated &&
                        <span className="navbar-text"></span>
                    }

                </div>
            </nav>
        )
    }
}