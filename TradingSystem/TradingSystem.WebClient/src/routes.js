import React from "react";
import {Redirect, Route, Switch} from "react-router-dom";
import LoginPage from "./pages/Login/Login";
import Signup from "./pages/Signup/Signup";
import {Home} from "./pages/mainPage/Home";
import {MyStores} from "./pages/Stores/MyStores";
import {Store} from "./pages/Stores/Store";
import StoreCreateForm from "./pages/Stores/StoreCreateForm";
import {ShoppingCart} from "./pages/ShoppingCart/ShoppingCart";
import {GlobalContext} from "./globalContext";
import {searchStore} from "./pages/Stores/searchStore";
import {StoreProductsUserView} from "./pages/Stores/StoreProductsUserView";
import {UserHistory} from "./pages/History/UserHistory";
import {HistoryProductsList} from "./pages/History/HistoryProductsList";
import {AdminActions} from "./pages/Admin/AdminActions";
import {AdminHistory} from "./pages/Admin/AdminHistory";
import {Discounts} from "./pages/Stores/Discount/Discounts";
import {Policy} from "./pages/Stores/Policy/Policy";
import {UserBids} from "./pages/UserBids/userBids";
import {StoreBids} from "./pages/Stores/StoreBids/storeBids";
import {Statistics} from "./pages/Admin/Statistics";

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
            <Route path={"/Discounts"} component={Discounts} />
            <Route path={"/Policy"} component={Policy} />
            <Route path={"/userBids"} component={UserBids} />
            <Route path={"/storeBids"} component={StoreBids} />
            <Route path={"/statistics"} component={Statistics} />
        </Switch>
    );
}