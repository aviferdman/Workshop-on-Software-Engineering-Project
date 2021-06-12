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
            myPermissions: null,
            ready: false,
        };
        this.storeId = this.props.match.params.storeId;
    }

    async componentDidMount() {
        await Promise.all([
            this.fetchStoreProducts(),
            this.fetchBids(),
            this.fetchMyStorePermissions(),
        ]);
        this.setState({
            ready: true,
        });
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

    async fetchMyStorePermissions() {
        await api.stores.permissions.mine(this.context.username, this.storeId)
            .then(permissions => {
                this.setState({
                    myPermissions: {
                        role: permissions.role,
                        actions: util.arrayToHashset(permissions.permissions),
                    },
                });
            }, alertRequestError_default);
    }

    render() {
        if (!this.state.ready) {
            return null;
        }

        return (
            <main>

                <div className="store-bids-grid">

                    {/*top grid - simple discounts*/}
                    <div  className="store-bids-grid-simple">
                        <h2> Store Bids </h2>
                        <BidRecordsStore
                            bidRecords={this.state.bids}
                            storeProductsMap={this.state.storeProductsMap}
                            storeId={this.storeId}
                            myPermissions={this.state.myPermissions}
                        />

                    </div>



                </div>

            </main>
        )
    }
}

StoreBids.contextType = GlobalContext;
