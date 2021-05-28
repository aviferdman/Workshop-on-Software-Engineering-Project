import React, {Component} from 'react';
import './StoreProducts.css';
import Products from "../../components/Products";
import AddProduct from "../../components/AddProduct";
import {GlobalContext} from "../../globalContext";
import {alertRequestError_default} from "../../utils";
import * as api from "../../api";

export class StoreProducts extends Component {
    constructor(props) {
        super(props);
        this.state = {
            products: []
        };
    }

    async componentDidMount() {
        await this.fetchProducts();
    }

    async fetchProducts() {
        let response;
        try {
            response = await api.stores.info(this.props.match.params.storeId);
        }
        catch (e) {
            alertRequestError_default(e);
            return;
        }

        this.setState({
            products: response.products
        });
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
                              onProductRemoved={this.onProductRemoved} onProductEdited={this.onProductEdited} />
                </div>

                <div className="bottom-row">

                    <div className="center-add-product">
                        <AddProduct storeId={this.props.match.params.storeId} onProductAdded={this.onProductAdded} />
                    </div>

                </div>

            </main>
        );
    }
}

StoreProducts.contextType = GlobalContext;