import React from "react";
import { Route, Switch, Redirect } from "react-router-dom";
import LoginPage from "./pages/Login/Login";
import Signup from "./pages/Signup/Signup";
import {Home} from "./pages/mainPage/Home";
import {MyStores} from "./pages/Stores/MyStores";
import Store from "./pages/Stores/Store";
import StoreProducts from "./pages/Stores/StoreProducts";
import StoreStaff from "./pages/Stores/StoreStaff";
import StoreHistory from "./pages/Stores/StoreHistory";



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
            <Route path={"/store"}>
                <Store />
            </Route>
            <Route path={"/storeProducts"}>
                <StoreProducts />
            </Route>
            <Route path={"/storeStaff"}>
                <StoreStaff />
            </Route>

            <Route path={"/storeHistory"}>
                <StoreHistory/>
            </Route>

        </Switch>
    );
}