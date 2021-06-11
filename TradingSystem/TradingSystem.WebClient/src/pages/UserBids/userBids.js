import React from "react";
import './userBids.css';
import {GlobalContext} from "../../globalContext";
import Header from "../../header";
import BidRecordsUser from "./BidRecordsUser";
import * as api from '../../api'
import {alertRequestError_default} from "../../utils";

export class UserBids extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            bids: null,
        };
    }

    async componentDidMount() {
        await this.fetchBids();
    }

    async fetchBids() {
        await api.stores.bids.mine(this.context.username)
            .then(bids => {
                this.setState({
                    bids: bids,
                });
            }, alertRequestError_default)
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
                            <BidRecordsUser bidRecords={this.state.bids} />
                        </div>

                    </div>

                </main>

                <footer> End of Stores</footer>
            </div>
        )
    }
}

UserBids.contextType = GlobalContext;
