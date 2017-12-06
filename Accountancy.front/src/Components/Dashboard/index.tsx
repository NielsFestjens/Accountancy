import * as React from 'react';
import { Component, PropTypes } from 'react';
import { Link } from 'react-router-dom';

export interface IDashboardProps {
}

export default class Dashboard extends Component<IDashboardProps> {

    render() {
        const props = this.props

        return (
            <div>
                <h2>Le dashboard</h2>
            </div>
        )
    }
}