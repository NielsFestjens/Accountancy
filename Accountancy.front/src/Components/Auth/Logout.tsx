import * as React from 'react'
import { Component } from 'react'

export interface ILogoutProps {
    onLogoutClick: () => void;
}

export default class Logout extends Component<ILogoutProps> {

  render() {
    const { onLogoutClick } = this.props

    return (
      <button onClick={() => onLogoutClick()} className="btn btn-primary">
        Logout
      </button>
    )
  }

}