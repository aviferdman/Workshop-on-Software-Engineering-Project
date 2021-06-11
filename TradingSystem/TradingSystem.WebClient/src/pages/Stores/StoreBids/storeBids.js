import React from "react";
import './storeBids.css';
import {GlobalContext} from "../../../globalContext";
import Header from "../../../header";
import DataSimple from "../../../data/bidsData.json"
import BidRecords from "../../UserBids/BidRecords";
import AddPolicy from "../Policy/AddPolicy";
import AddBid from "./AddBid";


export class StoreBids extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            name: "",
            bids: DataSimple.bids
        };
    }

    render() {
        return (
            <div className="grid-container">
                <Header />

                <main>

                    <div className="store-bids-grid">

                        {/*top grid - simple discounts*/}
                        <div  className="store-bids-grid-simple">
                            <h2> Store Bids </h2>
                            <BidRecords bidRecords={this.state.bids}  />

                        </div>



                    </div>

                </main>

                <footer> End of Stores</footer>
            </div>
        )
    }
}

StoreBids.contextType = GlobalContext;
