import * as React from 'react'
import { Component, PropTypes } from 'react'
import { connect } from 'react-redux'
import { bindActionCreators, Dispatch } from "redux";

import * as Actions from '../actions'
import Login from './Login'
import Navbar from './Navbar'
import Quotes from './Quotes'

interface IAppStateProps {
  dispatch?: (action: any) => void;
  quote: string;
  isAuthenticated: boolean;
  errorMessage: string;
  isSecretQuote: boolean;
}

interface IAppDispatchProps { }

type IAppProps = IAppStateProps & IAppDispatchProps;

function mapStateToProps(state: any): IAppStateProps {

  const { quotes, auth } = state
  const { quote, authenticated } = quotes
  const { isAuthenticated, errorMessage } = auth

  return {
    quote,
    isSecretQuote: authenticated,
    isAuthenticated,
    errorMessage
  }
}

class App extends Component<IAppProps> {
  render() {
    const { dispatch, quote, isAuthenticated, errorMessage, isSecretQuote } = this.props
    return (
      <div>
        <Navbar
          isAuthenticated={isAuthenticated}
          errorMessage={errorMessage}
          dispatch={dispatch}
        />
        <div className='container'>
          <Quotes
            onQuoteClick={() => dispatch(Actions.fetchQuote())}
            onSecretQuoteClick={() => dispatch(Actions.fetchSecretQuote())}
            isAuthenticated={isAuthenticated}
            quote={quote}
            isSecretQuote={isSecretQuote}
          />
        </div>
      </div>
    )
  }
}
export default connect(mapStateToProps)(App);