import React, {Component} from 'react';
import './Store.css';
import {Route, Switch} from "react-router-dom";
import {GlobalContext} from "../../globalContext";
import Header from "../../header";
import {StoreProducts} from "./StoreProducts";
import {StoreStaff} from "./StoreStaff";
import {StoreHistory} from "./StoreHistory";
import {StoreProductsUserView} from './StoreProductsUserView'
import * as api from "../../api";
import * as util from "../../utils";
import {alertRequestError_default} from "../../utils";
import StoreRestrictedComponentCustom from "../../components/StoreRestrictedComponentCustom";
import {Discounts} from "./Discount/Discounts";
import {Policy} from "./Policy/Policy";

class StoreContent extends Component {
    constructor(props) {
        super(props);
        this.state = {
            name: '',
            myPermissions: {
                role: 'guest',
                actions: {},
            },
        };
        this.storeId = this.props.match.params.storeId;
    }

    async componentDidMount() {
        let promise_storeInfo = api.stores.info(this.storeId)
            .then(storeInfo => {
                this.setState({
                    name: storeInfo.name,
                });
            }, alertRequestError_default);
        let promise_storePermissions = api.stores.permissions.mine(this.context.username, this.storeId)
            .then(permissions => {
                this.setState({
                    myPermissions: {
                        role: permissions.role,
                        actions: util.arrayToHashset(permissions.permissions),
                    },
                });
            }, alertRequestError_default);
        await Promise.all([promise_storeInfo, promise_storePermissions]);
    }

    onNavigationButtonClick = route => e => {
        this.props.history.push(`/store/${route}/${this.storeId}`)
    }

    render() {
        return (
            <main className="main-conatiner">
                <div>
                    <h2>{this.state.name}</h2>
                </div>

                <div className="internal-conatiner store-container">
                    <StoreRestrictedComponentCustom
                        permissions={this.state.myPermissions}
                        allowedActions={[
                            api.data.stores.permissions.addProduct,
                            api.data.stores.permissions.editProduct,
                            api.data.stores.permissions.removeProduct,
                        ]}
                        render={() => (
                            <button className="button-view" onClick={this.onNavigationButtonClick('products')}>Store Products Management</button>
                        )} />
                    <button className="button-view" onClick={this.onNavigationButtonClick('productsView')}>Store Products</button>
                    <StoreRestrictedComponentCustom
                        permissions={this.state.myPermissions}
                        allowedActions={[api.data.stores.permissions.editDiscount,]}
                        render={() => (
                            <button className="button-view" onClick={this.onNavigationButtonClick('discounts')}>Store Discounts</button>
                        )} />
                    <StoreRestrictedComponentCustom
                        permissions={this.state.myPermissions}
                        allowedActions={[api.data.stores.permissions.editPolicy,]}
                        render={() => (
                            <button className="button-view" onClick={this.onNavigationButtonClick('policy')}>Store Policy</button>
                        )} />
                    <StoreRestrictedComponentCustom
                        permissions={this.state.myPermissions}
                        allowedActions={[api.data.stores.permissions.getPersonnelInfo,]}
                        render={() => (
                            <button className="button-view" onClick={this.onNavigationButtonClick('staff')}>Store Staff</button>
                        )} />
                    <StoreRestrictedComponentCustom
                        permissions={this.state.myPermissions}
                        allowedActions={[api.data.stores.permissions.getShopHistory,]}
                        render={() => (
                            <button className="button-view" onClick={this.onNavigationButtonClick('history')}>Store History</button>
                        )} />
                </div>
            </main>
        );
    }
}

export class Store extends Component {
    render() {
        return (
            <div className="grid-container">
                <Header />

                <Switch>
                    <Route path={`${this.props.match.path}/products/:storeId`} component={StoreProducts} />
                    <Route path={`${this.props.match.path}/productsView/:storeId`} component={StoreProductsUserView} />
                    <Route path={`${this.props.match.path}/staff/:storeId`} component={StoreStaff} />
                    <Route path={`${this.props.match.path}/history/:storeId`} component={StoreHistory} />
                    <Route path={`${this.props.match.path}/discounts/:storeId`} component={Discounts} />
                    <Route path={`${this.props.match.path}/policy/:storeId`} component={Policy} />
                    <Route path={`${this.props.match.path}/:storeId`} component={StoreContent} />
                    <Route path={this.props.match.path}>
                        <h3 className='center-screen'>No store selected</h3>
                    </Route>
                </Switch>
                <footer>End of Store</footer>
            </div>
        )
    }
}

StoreContent.contextType = GlobalContext;
