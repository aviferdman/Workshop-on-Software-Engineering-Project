import React from "react";
import { Route, Switch, Redirect } from "react-router-dom";
import LoginPage from "./pages/Login/Login";
import Signup from "./pages/Signup/Signup";
import {Home} from "./pages/mainPage/Home";
import {MyStores} from "./pages/Stores/MyStores";
import {Store} from "./pages/Stores/Store";
import {StoreProducts} from "./pages/Stores/StoreProducts";
import StoreCreateForm from "./pages/Stores/StoreCreateForm";
import {StoreStaff} from "./pages/Stores/StoreStaff";
import {StoreHistory} from "./pages/Stores/StoreHistory";
import {ShoppingCart} from "./pages/ShoppingCart/ShoppingCart";

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
            <Route path={"/storeStaff"} component={StoreStaff} />
            <Route path={"/storeHistory"} component={StoreHistory} />
            <Route path={"/storeCreate"} component={StoreCreateForm} />
            <Route path={"/ShoppingCart"} component={ShoppingCart} />

        </Switch>
    );
}