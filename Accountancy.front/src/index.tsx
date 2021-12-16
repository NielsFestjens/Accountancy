import * as ReactDOM from "react-dom";
import { BrowserRouter } from "react-router-dom";

import 'bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css';

import App from "Components/App";
import 'Styles/default.css';
import 'font-awesome/css/font-awesome.css';

ReactDOM.render(
    <BrowserRouter>
        <App />
    </BrowserRouter>,
    document.getElementById("root")
);