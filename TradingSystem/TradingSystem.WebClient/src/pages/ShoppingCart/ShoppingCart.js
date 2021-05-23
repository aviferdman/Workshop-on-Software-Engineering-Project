import React, {Component} from 'react';
import * as HiIcons from "react-icons/hi";
import Navbar from "../../components/Navbar/Navbar";
import './ShoppingCart.css';
import CartProducts from "../../components/CartProducts";
import Purchase from "../../components/Purchase";
import formatCurrency from "../mainPage/currency";
import {Link} from "react-router-dom";
import {GlobalContext} from "../../globalContext";
import axios from "axios";
import {alertRequestError_default} from "../../utils";

export class ShoppingCart extends Component {
    constructor(props) {
        super(props);
        this.state = {
            name: "",
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

    render() {
        return (
            <div className="grid-container">
                <header className="header-container">
                    <a href="/">E - commerce Application</a>
                    <div>
                        <h3>{this.context.isLoggedIn ? this.context.username : ''}</h3>
                    </div>


                    <Link
                        className="icons"
                        to={{
                            pathname: "/ShoppingCart"
                        }}
                    >
                        <HiIcons.HiShoppingCart />
                    </Link>


                    <Navbar/>

                </header>

                <main className="store-products-main-conatiner">

                    <div>
                        <CartProducts products={this.state.products} />
                    </div>

                    {/* TODO: get total price from server */}
                    <div className="total">
                        Total: {" "}
                        {formatCurrency(this.state.products.reduce((a,c) => a + (c.price * c.count), 0))}
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