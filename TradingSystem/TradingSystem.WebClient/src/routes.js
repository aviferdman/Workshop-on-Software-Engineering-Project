import React from "react";
import { Route, Switch, Redirect } from "react-router-dom";
import LoginPage from "./pages/Login/Login";
import Signup from "./pages/Signup/Signup";
import {Home} from "./pages/mainPage/Home";
import {MyStores} from "./pages/Stores/MyStores";
import Store from "./pages/Stores/Store";
import StoreProducts from "./pages/Stores/StoreProducts";
import StoreCreateForm from "./pages/Stores/StoreCreateForm";



export default function Routes() {
    return (
        <Switch>
            <Route exact path="/">
                <Redirect exact from="/" to="/login" />
            </Route>
            <Route path={"/login"} component={LoginPage} />
            <Route path={"/signup"} component={Signup} />
            <Route path={"/home"} component={Home} />
            <Route path={"/myStores"} component={MyStores} />
            <Route path={"/store"} component={Store} />
            <Route path={"/storeProducts"} component={StoreProducts} />
            <Route path={"/storeCreate"} component={StoreCreateForm} />
        </Switch>
    );
}