import React from "react";
import './storeBids.css';
import {GlobalContext} from "../../../globalContext";
import Header from "../../../header";
import DataSimple from "../../../data/bidsData.json"
import BidRecord from "../../UserBids/BidRecord";
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
                            <BidRecord bidRecords={this.state.bids}  />

                        </div>

                        <div className="store-bids-grid-button">
                            <div className="center-btn-st">
                                <AddBid />
                            </div>
                        </div>

                    </div>

                </main>

                <footer> End of Stores</footer>
            </div>
        )
    }
}

StoreBids.contextType = GlobalContext;
