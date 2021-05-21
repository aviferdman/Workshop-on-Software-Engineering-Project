import React, {Component} from 'react';
import * as HiIcons from "react-icons/hi";
import Navbar from "../../components/Navbar/Navbar";
import './StoreProducts.css';
import data from "../../data/productData.json";
import Products from "../../components/Products";
import AddProduct from "../../components/AddProduct";

class StoreProducts extends Component {

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
                    <div></div>
                    <button className="icons">
                        <HiIcons.HiShoppingCart />
                    </button>
                    <Navbar></Navbar>
                </header>

                <main className="store-products-main-conatiner">

                    <div>
                        <Products products={this.state.products} ></Products>

                    </div>

                    <div className="bottom-row">
                        <AddProduct></AddProduct>
                    </div>

                </main>
                <footer> End of Store</footer>
            </div>
        )
    }
}

export default StoreProducts;