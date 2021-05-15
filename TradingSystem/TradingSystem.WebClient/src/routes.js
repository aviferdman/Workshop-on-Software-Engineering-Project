import React from "react";
import { Route, Switch, Redirect } from "react-router-dom";
import LoginPage from "./pages/Login/Login";
import Signup from "./pages/Signup/Signup";
import {Home} from "./pages/mainPage/Home";
import {MyStores} from "./pages/Stores/MyStores";


export default function Routes() {
    return (
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
            <Route path={"/myStores"}>
                <MyStores />
            </Route>

        </Switch>
    );
}