import React, {Component} from 'react';
import './ShoppingCart.css';
import CartProducts from "../../components/CartProducts";
import Purchase from "../../components/Purchase";
import {GlobalContext} from "../../globalContext";
import axios from "axios";
import {alertRequestError_default} from "../../utils";
import Header from "../../header";
import formatCurrency from "../mainPage/currency";

export class ShoppingCart extends Component {
    constructor(props) {
        super(props);
        this.state = {
            totalPrice: null,
            products: [],
        };
    }

    async componentDidMount() {
        await this.fetchShoppingCart();
    }

    fetchShoppingCart = async (errorFunc) => {
        axios.post('/ShoppingCart/MyShoppingCart', {
            username: this.context.username,
        }).then(response => {
            this.setState({
                totalPrice: response.data.totalPrice,
                products: this.flattenShoppingBaskets(response.data.shoppingBaskets),
            });
        }, (e) => {
            if (errorFunc) {
                errorFunc(e);
            }
            alertRequestError_default(e);
        });
    }

    flattenShoppingBaskets(shoppingBaskets) {
        let products = [];
        shoppingBaskets.forEach(shoppingBasket => {
            let store = {
                id: shoppingBasket.id,
                name: shoppingBasket.name,
            };
            shoppingBasket.products.forEach(product => {
                product.store = store;
                product.__visible = true;
                products.push(product);
            });
        });
        return products;
    }

    onPurchaseSuccess = () => {
        alert('purchase successful');
        this.props.history.push('/home');
    }

    onRemoveProduct = async product => {
        product.__visible = false;
        this.setState({
            ...this.state
        });
        await this.fetchShoppingCart(e => {
            product.__visible = true;
            this.setState({
                ...this.state
            });
        });
    }

    onEditProduct = async product => {
        await this.fetchShoppingCart();
    }

    render() {
        return (
            <div className="grid-container">
                <Header />

                <main className="store-products-main-conatiner">

                    <div>
                        <CartProducts products={this.state.products}
                                      onEditProduct={this.onEditProduct}
                                      onRemoveProduct={this.onRemoveProduct} />
                    </div>

                    <div className="total">
                        <h2>Total price of all products: </h2>
                        {this.state.totalPrice == null ? null : (
                            <h1>{formatCurrency(this.state.totalPrice)} </h1>
                        )}
                    </div>

                    <div className="bottom-row">

                        <div className="grid-item">
                            <Purchase onSuccess={this.onPurchaseSuccess}/>
                        </div>

                    </div>

                </main>
                <footer> End of Store</footer>
            </div>
        )
    }
}

ShoppingCart.contextType = GlobalContext;