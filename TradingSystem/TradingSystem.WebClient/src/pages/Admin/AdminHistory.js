import React, {Component} from 'react';
import './AdminHistory.css';
import {GlobalContext} from "../../globalContext";
import Header from "../../header";
import SearchBar from "../../components/searchBar";
import History from "../../components/History";
import * as api from "../../api";
import {alertRequestError_default} from "../../utils";


export class AdminHistory extends Component {
    constructor(props) {
        super(props);
        this.state = {
            name: "",
            historyRecords: null,
        };
    }

    async componentDidMount() {
        console.log('admin history');
        await api.history.all(this.context.username)
            .then(historyRecords => {
                console.log('received history:', historyRecords);
                this.setState({
                    historyRecords: historyRecords,
                });
            }, alertRequestError_default);
    }

    render() {
        return (
            <div className="grid-container">
                <Header />

                <main className="admin-hist-cont">
                    <div className="admin-search-store-grid">
                        <div className="admin-search-store-element">
                            <div>
                                <SearchBar />
                                {" "}
                            </div>

                            <div className="filter-category">
                                Search History By:{" "}
                                <select value={this.state.searchCategory} onChange={this.onSearchCategoryChange}>
                                    <option value="">All</option>
                                    <option value="Store">Store</option>
                                    <option value="User">User</option>

                                </select>
                            </div>

                        </div>

                        <div>
                            {this.state.historyRecords == null ? null : (
                                <History  historyRecords={this.state.historyRecords} history={this.props.history} />
                            )}
                        </div>

                    </div>
                </main>

                <footer> End of Page</footer>
            </div>
        )
    }
}

AdminHistory.contextType = GlobalContext;
