import React, {Component} from 'react';
import './StoreHistory.css';
import History from "../../components/History";
import {GlobalContext} from "../../globalContext";
import * as api from "../../api";
import {alertRequestError_default} from "../../utils";

export class StoreHistory extends Component {
    constructor(props) {
        super(props);
        this.state = {
            name: "",
            historyRecords: null,
        };
        this.storeId = this.props.match.params.storeId;
    }

    async componentDidMount() {
        let promise_history = api.history.ofStore(this.context.username, this.storeId)
            .then(historyRecords => {
                this.setState({
                    historyRecords: historyRecords,
                });
            }, alertRequestError_default);
        let promise_info = api.stores.info(this.storeId)
            .then(storeInfo => {
                this.setState({
                    name: storeInfo.name,
                });
            }, alertRequestError_default);
        await Promise.all([promise_info, promise_history]);
    }

    render() {
        return (
            <main className="store-products-main-conatiner">
                <h2>'{this.state.name}' History</h2>
                <div>
                    {this.state.historyRecords == null ? null : (
                        <div className="history_s">
                            <History historyRecords={this.state.historyRecords} history={this.props.history} storeId={this.storeId} />
                        </div>
                    )}
                </div>

                <div className="bottom-row">


                </div>

            </main>
        )
    }
}

StoreHistory.contextType = GlobalContext;