import * as React from 'react'

export interface ILogoutProps {
    onLogoutClick: () => void;
}

const Logout = (props: ILogoutProps) => {
    const { onLogoutClick } = props;

    return (
      <button onClick={() => onLogoutClick()} className="btn btn-primary">
        Logout
      </button>
    );
}

export default Logout;