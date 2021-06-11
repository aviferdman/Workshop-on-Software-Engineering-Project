import React from "react";
import './storeBids.css';
import {GlobalContext} from "../../../globalContext";
import Header from "../../../header";
import DataSimple from "../../../data/bidsData.json"
import BidRecords from "../../UserBids/BidRecords";
import * as api from "../../../api";
import {alertRequestError_default} from "../../../utils";


export class StoreBids extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            bids: null,
        };
        this.storeId = this.props.match.params.storeId;
    }

    async componentDidMount() {
        await this.fetchBids();
    }

    async fetchBids() {
        await api.stores.bids.ofStore(this.context.username, this.storeId)
            .then(bids => {
                this.setState({
                    bids: bids,
                });
            }, alertRequestError_default)
    }

    render() {
        return (
            <main>

                <div className="store-bids-grid">

                    {/*top grid - simple discounts*/}
                    <div  className="store-bids-grid-simple">
                        <h2> Store Bids </h2>
                        <BidRecords bidRecords={this.state.bids}  />

                    </div>



                </div>

            </main>
        )
    }
}

StoreBids.contextType = GlobalContext;
