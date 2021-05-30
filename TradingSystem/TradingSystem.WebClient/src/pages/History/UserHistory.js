import React from "react";
import './UserHistory.css';
import {GlobalContext} from "../../globalContext";
import Header from "../../header";
import History from "../../components/History";
import * as api from "../../api";
import {alertRequestError_default} from "../../utils";

export class UserHistory extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            name: "",
            historyRecords: null,
        };
    }

    async componentDidMount() {
         await api.history.mine(this.context.username)
             .then(historyRecords => {
                 this.setState({
                     historyRecords: historyRecords,
                 });
             }, alertRequestError_default);
    }

    render() {
        return (
            <div className="grid-container">
                <Header />

                <main>

                    <div className="user-history-grid">
                        <div className="history-view-flex">
                            {this.state.historyRecords == null ? null : (
                                <History historyRecords={this.state.historyRecords} history={this.props.history} username={this.context.username} />
                            )}
                        </div>

                    </div>

                </main>

                <footer> End of Stores</footer>
            </div>
        )
    }
}

UserHistory.contextType = GlobalContext;
