import { useLocation, useNavigate } from 'react-router';
import { Link } from 'react-router-dom';
import Login from 'Components/Auth/Login';
import { User } from 'Components/Auth/models';
import * as DataService from 'Components/Auth/DataService';

export interface IProps {
    isAuthenticated: boolean;
    setIsAuthenticated: (isAuthenticated: boolean) => void;
    user?: User;
    setUser: (user: User) => void;
    addNotificationError: (notification: string) => void;
}

const Navbar = (props: IProps) => {
    const { isAuthenticated, setIsAuthenticated, user, setUser, addNotificationError } = props;

    const location = useLocation();
    const navigate = useNavigate();

    const registerUser = async (username: string, password: string) => {
        setIsAuthenticated(false);
        const result = await DataService.register(addNotificationError, username, password);
        await handleAutoResponse(result, "register");
    }

    const loginUser = async (username: string, password: string) => {
        setIsAuthenticated(false);
        const result = await DataService.login(addNotificationError, username, password);
        await handleAutoResponse(result, "login");
    }

    const handleAutoResponse = async(result: { response: Response, content: any } | undefined, action: string) => {
        if (!result)
            return;
        
        if (!result.response.ok) {
            addNotificationError(`Could not ${action}: ` + result.content.error);
            return;
        }
        
        const response = await DataService.getLoggedInUser(addNotificationError);
        setIsAuthenticated(true);
        setUser(response.content.user);
        if (location.pathname === '/')
            navigate('/dashboard');
    }

    const logoutUser = async () => {
        await DataService.logout(addNotificationError);
        setIsAuthenticated(false);
    }

    return (
        <nav className="navbar navbar-expand-lg navbar-light bg-light">
            <button className="navbar-brand">Accountancy</button>
            
            <button className="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                <span className="navbar-toggler-icon"></span>
            </button>
            
            <div className="collapse navbar-collapse" id="navbarSupportedContent">

                {!isAuthenticated &&
                    <Login
                        onLoginClick={(username: string, password: string) => loginUser(username, password)}
                        onRegisterClick={(username: string, password: string) => registerUser(username, password)}
                    />
                }

                {isAuthenticated &&
                    <ul className="navbar-nav mr-auto">
                        <li className="nav-item">
                            <Link to="/dashboard" className="nav-link">Dashboard</Link>
                        </li>
                        <li className="nav-item dropdown">
                            <button className="nav-link dropdown-toggle" id="navbarDropdown" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                {user?.username}
                            </button>
                            <div className="dropdown-menu" aria-labelledby="navbarDropdown">
                                <button className="dropdown-item" onClick={() => logoutUser()}>Logout</button>
                            </div>
                        </li>
                    </ul>
                }

                {props.isAuthenticated &&
                    <span className="navbar-text"></span>
                }

            </div>
        </nav>
    );
}

export default Navbar;