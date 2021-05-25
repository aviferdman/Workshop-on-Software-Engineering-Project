import React from "react";
import './StoreProductsUserView.css';
import {GlobalContext} from "../../globalContext";
import Header from "../../header";
import Data from "../../data/productData.json"
import HomeProducts from "../../components/HomeProducts";

export class StoreProductsUserView extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            products: Data.products
        };
    }


    render() {
        return (
            <div className="grid-container">
                <Header />

                <main>
                    
                        <div className="products-view-flex">
                            <HomeProducts  products={this.state.products}/>
                        </div>

                </main>

                <footer> End of Stores</footer>
            </div>
        )
    }
}

StoreProductsUserView.contextType = GlobalContext;
