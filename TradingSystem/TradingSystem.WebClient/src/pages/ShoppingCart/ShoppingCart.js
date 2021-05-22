
import React, {Component} from 'react';
import * as HiIcons from "react-icons/hi";
import Navbar from "../../components/Navbar/Navbar";
import './ShoppingCart.css';
import data from "../../data/productData.json";
import CartProducts from "../../components/CartProducts";
import Purchase from "../../components/Purchase";
import formatCurrency from "../mainPage/currency";
import {Link} from "react-router-dom";
import {GlobalContext} from "../../globalContext";

export class ShoppingCart extends Component {

    constructor(props) {
        super(props);
        this.state = {
            name: "",
            products: data.products
        };
    }
    render() {
        return (
            <div className="grid-container">
                <header className="header-container" >
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


                    <Navbar></Navbar>

                </header>

                <main className="store-products-main-conatiner ">

                    <div>
                        <CartProducts products={this.state.products} ></CartProducts>
                    </div>

                    <div className="total">
                        Total: {" "}
                        {formatCurrency(this.state.products.reduce((a,c) => a + (c.price * c.count), 0))}
                    </div>

                    <div className="bottom-row">

                        <div className="grid-item">
                            <Purchase></Purchase>
                        </div>

                    </div>


                </main>
                <footer> End of Store</footer>
            </div>
        )
    }
}

ShoppingCart.contextType = GlobalContext;