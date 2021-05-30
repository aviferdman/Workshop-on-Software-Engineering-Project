import React from "react";
import './HistoryProductsList.css';
import {GlobalContext} from "../../globalContext";
import Header from "../../header";
import HistoryProducts from "./HistoryProducts";
import formatCurrency from "../mainPage/currency";
import * as api from "../../api";
import {alertRequestError_default} from "../../utils";

export class HistoryProductsList extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            products: null,
            username: null,
            storeId: null,
            storeName: null,
        };
    }

    async componentDidMount() {
        let urlSearchParams = new URLSearchParams(this.props.location.search);
        let paymentId = urlSearchParams.get('paymentId');
        if (paymentId == null) {
            console.error('history products: null payment id');
            return;
        }

        let username = urlSearchParams.get('user') == null ? null : this.context.username;
        let storeId = urlSearchParams.get('storeId');

        let promise = null;
        if (this.props.storeId != null && this.props.username != null) {
            console.error('invalid history type');
            return;
        }
        else if (storeId == null && username != null) {
            promise = api.history.mineSpecific(username, paymentId);
        }
        else if (storeId != null && username == null) {
            promise = api.history.ofStoreSpecific(this.context.username, storeId, paymentId);
        }
        else {
            promise = api.history.allSpecific(this.context.username, paymentId);
        }

        await promise.then(historyRecord => {
            this.setState({
                username: username == null ? historyRecord.username : null,
                storeId: storeId == null ? historyRecord.storeId : null,
                storeName: storeId == null ? historyRecord.storeName : null,
                products: historyRecord.products,
            });
        }, alertRequestError_default);
    }

    render() {
        return (
            <div className="grid-container">
                <Header />

                <main>

                    {this.state.products == null ? null : (
                        <div className="history-products-grid">
                            <div className="history-products-view-flex">
                                <HistoryProducts  products={this.state.products}/>
                            </div>

                            <div className="total">
                                <h2>Total price of all products: </h2>
                                <h1>{formatCurrency(this.state.products.reduce((a,c) => a + (c.price * c.quantity), 0))} </h1>
                            </div>

                        </div>
                    )}

                </main>

                <footer> End of Stores</footer>
            </div>
        )
    }
}

HistoryProductsList.contextType = GlobalContext;
