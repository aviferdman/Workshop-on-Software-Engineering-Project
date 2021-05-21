import React, {Component} from 'react';
import * as HiIcons from "react-icons/hi";
import Navbar from "../../components/Navbar/Navbar";
import './StoreProducts.css';
import data from "../../data/productData.json";
import Products from "../../components/Products";
import AddProduct from "../../components/AddProduct";
import {Route, Switch} from "react-router-dom";

class StoreProductsContent extends Component {
    constructor(props) {
        super(props);
        this.state = {
            name: "",
            products: data.products
        };
    }

    render() {
        return (
            <main className="store-products-main-conatiner">

                <div>
                    <Products products={this.state.products} />
                </div>

                <div className="bottom-row">
                    <AddProduct storeId={this.props.match.params.storeId} history={this.props.history} />
                </div>

            </main>
        );
    }
}

class StoreProducts extends Component {
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

                <Switch>
                    <Route path={`${this.props.match.path}/:storeId`} component={StoreProductsContent} />
                    <Route path={this.props.match.path}>
                        <h3 className='center-screen'>No store selected</h3>
                    </Route>
                </Switch>
                <footer> End of Store</footer>
            </div>
        )
    }
}

export default StoreProducts;