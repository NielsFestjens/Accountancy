import { combineReducers } from 'redux';

import auth from 'Components/Auth/Reducers';
import notifications from 'Components/Blocks/Notifications/Reducers';

const reducers = combineReducers({
  auth,
  notifications
});

export default reducers;