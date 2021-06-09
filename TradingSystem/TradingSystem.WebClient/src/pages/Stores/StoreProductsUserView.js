import React from "react";
import './StoreProductsUserView.css';
import {GlobalContext} from "../../globalContext";
import Header from "../../header";
import HomeProducts from "../../components/HomeProducts";
import * as api from "../../api";
import {alertRequestError_default} from "../../utils";
import SearchBar from "../../components/searchBar";
import StoreListUserView from "./StoreListUserView";

export class StoreProductsUserView extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            products: [],
            cartProducts: {},
        };
        this.storeId = this.props.match.params.storeId;
    }

    async componentDidMount() {
        await api.stores.infoWithProducts(this.storeId)
            .then(storeInfo => {
                this.setState({
                    products: storeInfo.products
                });
            }, alertRequestError_default);
    }

    onAddToCart = product => {
        let cartProducts = this.state.cartProducts;
        if(!(product.id in cartProducts)){
            product._inCart = true;
            cartProducts[product.id] = product;
        }
        this.setState({
            cartProducts: cartProducts
        });
    }

    render() {
        return (

                <main>
                    <div>
                        <h2> Store Products </h2>
                    </div>

                    <div className="products-view-flex">
                        <HomeProducts  products={this.state.products}
                                       addToCart={this.onAddToCart}
                                       history={this.props.history}
                                       storeId={this.storeId} />
                    </div>
                </main>

        )
    }
}

StoreProductsUserView.contextType = GlobalContext;
