import React from "react";
import './UserHistory.css';
import {GlobalContext} from "../../globalContext";
import Header from "../../header";
import Data from "../../data/historyData.json"
import History from "../../components/History";

export class UserHistory extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            name: "",
            hist: Data.history
        };
    }


    render() {
        return (
            <div className="grid-container">
                <Header />

                <main>

                    <div className="user-history-grid">
                        <div className="history-view-flex">
                            <History  history={this.state.hist}/>
                        </div>

                    </div>

                </main>

                <footer> End of Stores</footer>
            </div>
        )
    }
}

UserHistory.contextType = GlobalContext;
