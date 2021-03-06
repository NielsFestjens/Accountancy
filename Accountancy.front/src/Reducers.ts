import { combineReducers } from 'redux';

import auth from 'Components/Auth/Reducers';
import notifications from 'Components/Blocks/Notifications/Reducers';
import dashboard from 'Components/Dashboard/Reducers';
import invoices from 'Components/Invoices/Reducers';

const reducers = combineReducers({
  auth,
  notifications,
  dashboard,
  invoices
});

export default reducers;