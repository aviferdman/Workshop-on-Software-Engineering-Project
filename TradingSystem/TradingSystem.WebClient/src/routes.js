import React from "react";
import {Redirect, Route, Switch} from "react-router-dom";
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
import {GlobalContext, UserRole} from "./globalContext";

class DefaultRoute extends React.Component {
    render() {
        let {...rest} = this.props;
        return (
            <Route {...rest} render={props => (
                <Redirect to={this.context.role ? '/home' : '/login'} />
            )} />
        );
    }
}
DefaultRoute.contextType = GlobalContext;

export default function Routes() {
    return (
        <Switch>
            <DefaultRoute exact path="/" />
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