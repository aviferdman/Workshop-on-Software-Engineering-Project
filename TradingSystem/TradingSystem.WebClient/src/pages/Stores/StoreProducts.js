import React, {Component} from 'react';
import './StoreProducts.css';
import Products from "../../components/Products";
import AddProduct from "../../components/AddProduct";
import {Route, Switch} from "react-router-dom";
import axios from "axios";
import {GlobalContext} from "../../globalContext";
import Header from "../../header";

class StoreProductsContent extends Component {
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
        try {
            let response = await axios.get('/Stores/Info', {
               params: {
                   storeId: this.props.match.params.storeId
               }
            });
            this.setState({
                products: response.data.products
            });
        }
        catch (e) {
            console.error("search error occurred: ", e);
        }
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
        let index = this.state.products.findIndex(p2 => {
            return p2.id === product.id;
        });
        this.state.products[index] = product;
        this.setState({
            ...this.state
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

export class StoreProducts extends Component {
    render() {
        return (
            <div className="grid-container">
                <Header />

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

StoreProducts.contextType = GlobalContext;