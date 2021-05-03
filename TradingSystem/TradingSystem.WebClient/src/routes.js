import React from "react";
import { Route, Switch, Redirect } from "react-router-dom";
import LoginPage from "./pages/Login";
import Signup from "./pages/Signup";
import {Home} from "./pages/mainPage/Home";

export default function Routes() {
    return (
        <Router>
            <Switch>
                <Route exact path="/">
                    <Redirect exact from="/" to="/login" />
                </Route>
                <Route path={"/login"}>
                    <LoginPage />
                </Route>
                <Route path={"/signup"}>
                    <Signup />
                </Route>
                <Route path={"/home"}>
                    <Home />
                </Route>
            </Switch>
        </Router>
    );
}