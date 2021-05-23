import React, {Component} from 'react';
import './StoreHistory.css';
import data from "../../data/historyData.json";
import History from "../../components/History";
import {GlobalContext} from "../../globalContext";
import Header from "../../header";

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
            <div className="grid-container">
                <Header />

                <main className="store-products-main-conatiner">

                    <div>
                        <History history={this.state.history} />
                    </div>

                    <div className="bottom-row">


                    </div>

                </main>
                <footer> End of Store</footer>
            </div>
        )
    }
}

StoreHistory.contextType = GlobalContext;