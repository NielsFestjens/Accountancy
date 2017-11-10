import { combineReducers } from 'redux';

import auth from 'Components/Auth/Reducers';
import quotes from 'Components/Quotes/Reducers';

const reducers = combineReducers({
  auth,
  quotes
});

export default reducers;