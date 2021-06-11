import React from "react";
import './storeBids.css';
import * as api from "../../../api";
import {alertRequestError_default} from "../../../utils";
import BidRecordsStore from "./BidRecordsStore";
import * as util from "../../../utils";
import {GlobalContext} from "../../../globalContext";

export class StoreBids extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            bids: null,
            storeName: null,
            storeProductsMap: null,
        };
        this.storeId = this.props.match.params.storeId;
    }

    async componentDidMount() {
        let promise_storeProducts = this.fetchStoreProducts();
        let promise_bids = this.fetchBids();
        await Promise.all([
            promise_storeProducts,
            promise_bids
        ]);
    }

    async fetchBids() {
        await api.stores.bids.ofStore(this.context.username, this.storeId)
            .then(bids => {
                this.setState({
                    bids: bids,
                });
            }, alertRequestError_default)
    }

    async fetchStoreProducts() {
        await api.stores.infoWithProducts(this.storeId)
            .then(storeInfo => {
                this.setState({
                    storeName: storeInfo.name,
                    storeProductsMap: util.arrayToMap(storeInfo.products, p => p.id),
                });
            }, alertRequestError_default);
    }

    render() {
        return (
            <main>

                <div className="store-bids-grid">

                    {/*top grid - simple discounts*/}
                    <div  className="store-bids-grid-simple">
                        <h2> Store Bids </h2>
                        <BidRecordsStore bidRecords={this.state.bids} storeProductsMap={this.state.storeProductsMap}  />

                    </div>



                </div>

            </main>
        )
    }
}

StoreBids.contextType = GlobalContext;
