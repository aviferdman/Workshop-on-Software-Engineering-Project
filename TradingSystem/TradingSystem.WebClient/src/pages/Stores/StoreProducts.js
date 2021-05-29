import React, {Component} from 'react';
import './StoreProducts.css';
import Products from "../../components/Products";
import AddProduct from "../../components/AddProduct";
import {GlobalContext} from "../../globalContext";
import * as util from "../../utils";
import {alertRequestError_default} from "../../utils";
import * as api from "../../api";
import StoreRestrictedComponentCustom from "../../components/StoreRestrictedComponentCustom";

export class StoreProducts extends Component {
    constructor(props) {
        super(props);
        this.state = {
            products: [],
            myPermissions: {
                role: 'guest',
                actions: {},
            }
        };
        this.storeId = this.props.match.params.storeId;
    }

    async componentDidMount() {
        await this.fetchProducts();
    }

    async fetchProducts() {
        let promise_storeInfo = api.stores.info(this.storeId)
            .then(storeInfo => {
                this.setState({
                    products: storeInfo.products
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

    onProductRemoved = product => {
        this.setState({
           products: this.state.products.filter(p2 => {
               return p2.id !== product.id;
           })
        });
    }

    onProductAdded = product => {
        this.state.products.push(product);
        this.setState({
            ...this.state
        });
    }

    onProductEdited = product => {
        this.setState({
            products: this.state.products.map(p2 => {
                return p2.id === product.id ? product : p2;
            }),
        });
    }

    render() {
        return (
            <main className="store-products-main-conatiner">

                <div>
                    <Products storeId={this.props.match.params.storeId}
                              products={this.state.products}
                              myPermissions={this.state.myPermissions}
                              onProductRemoved={this.onProductRemoved} onProductEdited={this.onProductEdited} />
                </div>

                <div className="bottom-row">

                    <div className="center-add-product">
                        <StoreRestrictedComponentCustom
                            permissions={this.state.myPermissions}
                            allowedActions={[api.data.stores.permissions.addProduct,]}
                            render={() => (
                                <AddProduct storeId={this.props.match.params.storeId}
                                            onProductAdded={this.onProductAdded} />
                            )} />
                    </div>

                </div>

            </main>
        );
    }
}

StoreProducts.contextType = GlobalContext;