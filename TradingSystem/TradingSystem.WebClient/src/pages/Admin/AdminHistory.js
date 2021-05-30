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
            historyRecordsFiltered: null,
            searchCategory: '',
            searchText: '',
            searchBarEnabled: false,
        };
    }

    async componentDidMount() {
        return await this.fetchAllHistory();
    }

    async fetchAllHistory() {
        return await api.history.all(this.context.username)
            .then(historyRecords => {
                this.setState({
                    historyRecords: historyRecords,
                    historyRecordsFiltered: historyRecords,
                });
            }, alertRequestError_default);
    }

    onSearchCategoryChange = e => {
        let category = e.target.value;
        let records = this.filter(this.state.historyRecords, this.state.searchText, category);
        if (records == null) {
            records = this.state.historyRecords;
        }
        this.setState({
            searchCategory: category,
            searchBarEnabled: category !== '',
            historyRecordsFiltered: records,
        });
    };

    onSearchTextChanged = e => {
        this.setState({
            searchText: e.target.value,
        });
    }

    filter = (historyRecords, searchText, category) => {
        if (!category) {
            return null;
        }

        searchText = searchText.toLowerCase();
        if(searchText === ""){
            return historyRecords;
        }
        else{
            return historyRecords.filter(h => {
                if (category === 'Store') {
                    return h.storeName.toLowerCase().includes(searchText);
                }
                else {
                    return h.username.toLowerCase().includes(searchText);
                }
            });
        }
    }

    onFilter = e => {
        let records = this.filter(this.state.historyRecords, this.state.searchText, this.state.searchCategory);
        if (records != null) {
            this.setState({
                historyRecordsFiltered: records,
            })
        }
    };

    render() {
        return (
            <div className="grid-container">
                <Header />

                <main className="admin-hist-cont">
                    <div className="admin-search-store-grid">
                        <div className="admin-search-store-element">
                            <div>
                                <SearchBar value={this.state.searchText} onChange={this.onSearchTextChanged}
                                           onSearch={this.onFilter} enabled={this.state.searchBarEnabled} />
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
                            {this.state.historyRecordsFiltered == null ? null : (
                                <History  historyRecords={this.state.historyRecordsFiltered} history={this.props.history} />
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
