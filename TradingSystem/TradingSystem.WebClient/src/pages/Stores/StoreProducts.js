import React, {Component} from 'react';
import * as HiIcons from "react-icons/hi";
import Navbar from "../../components/Navbar/Navbar";
import './StoreProducts.css';
import data from "../../data/productData.json";
import Products from "../../components/Products";
import AddProduct from "../../components/AddProduct";
import {Route, Switch} from "react-router-dom";

// TODO: fetch data from server
class StoreProductsContent extends Component {
    constructor(props) {
        super(props);
        this.state = {
            products: data.products
        };
    }

    onProductRemoved = product => {
        this.setState({
           products: this.state.products.filter(p2 => {
               return p2.id !== product.id;
           })
        });
    }

    render() {
        return (
            <main className="store-products-main-conatiner">

                <div>
                    <Products storeId={this.props.match.params.storeId}
                              products={this.state.products}
                              onProductRemoved={this.onProductRemoved} />
                </div>

                <div className="bottom-row">
                    <AddProduct storeId={this.props.match.params.storeId} />
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