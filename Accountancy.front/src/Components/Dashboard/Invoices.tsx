import * as React from 'react';
import { Component, PropTypes } from 'react';
import { Link } from 'react-router-dom';

import { Invoice } from './models';

export interface IINvoicesProps {
    invoices: Invoice[]
}

export default class Facturen extends Component<IINvoicesProps> {

    render() {
        const props = this.props

        return (
            <div>
                <h3>Facturen</h3>

            </div>
        )
    }
}