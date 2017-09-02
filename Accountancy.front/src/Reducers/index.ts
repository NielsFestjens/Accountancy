import { combineReducers } from 'redux';

import auth from './Auth';
import quotes from './Quotes';

const reducers = combineReducers({
  auth,
  quotes
});

export default reducers;