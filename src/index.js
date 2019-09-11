import React from "react";
import ReactDOM from "react-dom";
import { BrowserRouter as Router, Switch, Route } from "react-router-dom";
import "./index.css";
import App from "./App";
import Signin from "./components/Signin";
import Signup from "./components/Signup";
import Checkout from "./components/Checkout";
import * as serviceWorker from "./serviceWorker";

const Root = () => (
  <Router>
    <Switch>
      <Route component={App} exact path="/" />
      <Route component={Signin} exact path="/signin" />
      <Route component={Signup} exact path="/signup" />
      <Route component={Checkout} exact path="/checkout" />
    </Switch>
  </Router>
);

ReactDOM.render(<Root />, document.getElementById("root"));

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://bit.ly/CRA-PWA
serviceWorker.unregister();
