import React, {Component} from 'react';
import * as HiIcons from "react-icons/hi";
import Navbar from "../../components/Navbar/Navbar";
import './ShoppingCart.css';
import data from "../../data/productData.json";
import CartProducts from "../../components/CartProducts";

class ShoppingCart extends Component {

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
                <header className="header-container">
                    <a href="/">E - commerce Application</a>
                    <div/>
                    <button className="icons">
                        <HiIcons.HiShoppingCart />
                    </button>
                    <Navbar/>
                </header>

                <main className="store-products-main-conatiner">

                    <div>
                        <CartProducts products={this.state.products} />
                    </div>

                    <div className="bottom-row">


                    </div>
                </main>
                <footer> End of Store</footer>
            </div>
        )
    }
}

export default ShoppingCart;