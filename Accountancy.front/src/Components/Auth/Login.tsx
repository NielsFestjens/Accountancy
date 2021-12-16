import React, { useState } from 'react';

export interface ILoginProps {
    onLoginClick: (username: string, password: string) => void;
    onRegisterClick: (username: string, password: string) => void;
}

const Login = (props: ILoginProps) => {
    const { onLoginClick, onRegisterClick } = props;

    const [username, setUserName] = useState('');
    const [password, setPassword] = useState('');

    const handleLoginClick = () => onLoginClick(username.trim(), password.trim());
    const handleRegisterClick = () => onRegisterClick(username.trim(), password.trim());
    
    return (
        <div>
            <input type='text' value={username} onChange={x => setUserName(x.target.value)} className="form-control" style={{width:100, display:"inline"}} placeholder='Username'/>&nbsp;
            <input type='password' value={password} onChange={x => setPassword(x.target.value)}  className="form-control" style={{width:100, display:"inline"}} placeholder='Password'/>&nbsp;
            <button onClick={handleLoginClick} className="btn btn-primary">Login</button>&nbsp;
            <button onClick={handleRegisterClick} className="btn btn-primary">Register</button>
        </div>
    )
}

export default Login;