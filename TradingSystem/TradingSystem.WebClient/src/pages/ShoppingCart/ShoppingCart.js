import React, {Component} from 'react';
import './ShoppingCart.css';
import CartProducts from "../../components/CartProducts";
import Purchase from "../../components/Purchase";
import formatCurrency from "../mainPage/currency";
import {GlobalContext} from "../../globalContext";
import axios from "axios";
import {alertRequestError_default} from "../../utils";
import Header from "../../header";

export class ShoppingCart extends Component {
    constructor(props) {
        super(props);
        this.state = {
            products: []
        };
    }

    componentDidMount() {
        axios.post('/ShoppingCart/MyShoppingCart', {
            username: this.context.username,
        }).then(response => {
            this.setState({
                products: this.flattenShoppingBaskets(response.data),
            });
        }, alertRequestError_default);
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
                products.push(product);
            });
        });
        return products;
    }

    onPurchaseSuccess = () => {
        this.props.history.push('/home');
    }

    onRemoveProduct = product => {
        this.setState({
            products: this.state.products.filter(p => p.id !== product.id)
        });
    }

    render() {
        return (
            <div className="grid-container">
                <Header />

                <main className="store-products-main-conatiner">

                    <div>
                        <CartProducts products={this.state.products} onRemoveProduct={this.onRemoveProduct} />
                    </div>

                    {/* TODO: get total price from server */}
                    <div className="total">
                        <h2>Total price of all products: </h2>
                        <h1>{formatCurrency(this.state.products.reduce((a,c) => a + (c.price * c.quantity), 0))} </h1>
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