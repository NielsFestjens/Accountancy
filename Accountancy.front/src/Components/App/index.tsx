import { useState } from 'react'
import { Route, Routes, useNavigate } from 'react-router';
import { useLocation } from 'react-router';
import useAsyncEffect from 'use-async-effect';
import NotificationsContainer from 'Components/Blocks/Notifications';
import Notification, { NotificationType } from 'Components/Blocks/Notifications/Notification';
import { getLoggedInUser } from "Components/Auth/DataService";
import { User } from 'Components/Auth/models';
import uniqueId from 'Infrastructure/UniqueId';
import Dashboard from 'Components/Dashboard';
import Invoices from 'Components/Invoices';
import Navbar from 'Components/Layout/NavBar';

const App = () => {
    const [isAuthenticated, setIsAuthenticated] = useState(false);
    const [user, setUser] = useState<User | undefined>();
    const [notifications, setNotifications] = useState<Notification[]>([]);
    const addNotificationError = (message: string) => {
        setNotifications([...notifications, { id: uniqueId(), type: NotificationType.error, message }])
    }

    const location = useLocation();
    const navigate = useNavigate();

    useAsyncEffect(async isMounted => {
        const response = await getLoggedInUser(addNotificationError)
        if (!isMounted() || !response.content.user)
            return;

        setIsAuthenticated(true);
        setUser(response.content.user);
        if (location.pathname === '/')
            navigate('/dashboard');
    }, []);

    return (
        <div>
            <Navbar isAuthenticated={isAuthenticated} setIsAuthenticated={setIsAuthenticated} user={user} setUser={setUser} addNotificationError={addNotificationError} />
            <div className='container'>
                {isAuthenticated && 
                    <Routes>
                        <Route path="/dashboard" element={<Dashboard addNotificationError={addNotificationError} />} />
                        <Route path="/invoices" element={<Invoices addNotificationError={addNotificationError} />} />
                    </Routes>
                }
            </div>
            <NotificationsContainer notifications={notifications} setNotifications={setNotifications} />
        </div>
    )
}

export default App;
