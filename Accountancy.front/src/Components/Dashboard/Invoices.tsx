import * as React from 'react';
import { Component, PropTypes } from 'react';
import { Link } from 'react-router-dom';

import { InvoiceDto, InvoiceStatus } from './models';

export interface IINvoicesProps {
    invoices: InvoiceDto[]
}

export default class Facturen extends Component<IINvoicesProps> {

    render() {
        const props = this.props

        return (
            <div>
                <h3>Facturen</h3>
                <table>
                    <thead>
                        <tr>
                            <th>Naam</th>
                            <th>Status</th>
                        </tr>
                    </thead>
                    <tbody>
                        {this.props.invoices.map(x => 
                            <tr key={x.id}>
                                <td><a href={x.link} target="_blank">{x.name}</a></td>
                                <td>{InvoiceStatus[x.status]}</td>
                            </tr>
                        )}
                    </tbody>
                </table>
            </div>
        )
    }
}