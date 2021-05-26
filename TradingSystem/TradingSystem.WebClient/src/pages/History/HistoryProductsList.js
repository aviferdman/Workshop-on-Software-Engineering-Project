import React from "react";
import './HistoryProductsList.css';
import {GlobalContext} from "../../globalContext";
import Header from "../../header";
import Data from "../../data/productData.json"
import HomeProducts from "../../components/HomeProducts";
import HistoryProducts from "./HistoryProducts";
import formatCurrency from "../mainPage/currency";

export class HistoryProductsList extends React.Component {
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

                    <div className="history-products-grid">

                        <div className="history-products-view-flex">
                            <HistoryProducts  products={this.state.products}/>
                        </div>

                        <div className="total">
                            <h2>Total price of all products: </h2>
                            <h1>{formatCurrency(this.state.products.reduce((a,c) => a + (c.price * c.quantity), 0))} </h1>
                        </div>


                    </div>



                </main>

                <footer> End of Stores</footer>
            </div>
        )
    }
}

HistoryProductsList.contextType = GlobalContext;
