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
import {GlobalContext} from "./globalContext";
import {searchStore} from "./pages/Stores/searchStore";
import {StoreProductsUserView} from "./pages/Stores/StoreProductsUserView";
import {UserHistory} from "./pages/History/UserHistory";
import {HistoryProductsList} from "./pages/History/HistoryProductsList";
import {AdminActions} from "./pages/Admin/AdminActions";
import {AdminHistory} from "./pages/Admin/AdminHistory";

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
            <Route path={"/storeCreate"} component={StoreCreateForm} />
            <Route path={"/ShoppingCart"} component={ShoppingCart} />
            <Route path={"/searchStore"} component={searchStore} />
            <Route path={"/storeProductsUserView"} component={StoreProductsUserView} />
            <Route path={"/userHistory"} component={UserHistory} />
            <Route path={"/historyProducts"} component={HistoryProductsList} />
            <Route path={"/adminActions"} component={AdminActions} />
            <Route path={"/adminHistory"} component={AdminHistory} />
        </Switch>
    );
}