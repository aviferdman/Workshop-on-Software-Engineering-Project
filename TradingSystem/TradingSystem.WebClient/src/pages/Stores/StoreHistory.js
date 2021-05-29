import React, {Component} from 'react';
import './StoreHistory.css';
import data from "../../data/historyData.json";
import History from "../../components/History";
import {GlobalContext} from "../../globalContext";

export class StoreHistory extends Component {
    constructor(props) {
        super(props);
        this.state = {
            name: "",
            history: data.history
        };
    }

    render() {
        return (
            <main className="store-products-main-conatiner">

                <div>
                    <History history={this.state.history} />
                </div>

                <div className="bottom-row">


                </div>

            </main>
        )
    }
}

StoreHistory.contextType = GlobalContext;