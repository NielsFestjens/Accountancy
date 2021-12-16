import * as React from 'react';
import { Route, Routes } from 'react-router';
import Detail from './Detail';

type InvoicesProps = {
    addNotificationError: (message: string) => void;
}

const Invoices = (props: InvoicesProps) => {
    const { addNotificationError } = props;

    return (
        <Routes>
            <Route path="/Invoices/Invoice/:id"><Detail addNotificationError={addNotificationError} /></Route>
        </Routes>
    )
}
export default Invoices;