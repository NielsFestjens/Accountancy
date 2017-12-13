import { State as Notifications } from 'Components/Blocks/Notifications/Reducers';
import { State as Auth } from 'Components/Auth/Reducers';
import { State as Dashboard } from 'Components/Dashboard/Reducers';
import { State as Invoices } from 'Components/Invoices/Reducers';

export default class State {
    auth: Auth;
    dashboard: Dashboard;
    invoices: Invoices;
    notifications: Notifications;
}