import React from "react";
import './userBids.css';
import {GlobalContext} from "../../globalContext";
import Header from "../../header";
import DataSimple from "../../data/bidsData.json"
import BidRecord from "./BidRecord";

export class UserBids extends React.Component {
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

                    <div className="user-bids-grid">

                        {/*top grid - simple discounts*/}
                        <div  className="user-bids-grid-simple">
                            <h2> My Bids </h2>
                            <BidRecord bidRecords={this.state.bids}  />
                        </div>

                    </div>

                </main>

                <footer> End of Stores</footer>
            </div>
        )
    }
}

UserBids.contextType = GlobalContext;
