import React, {Component} from 'react';
import './AdminHistory.css';
import {GlobalContext} from "../../globalContext";
import Header from "../../header";
import SearchBar from "../../components/searchBar";
import Data from "../../data/historyData.json";
import History from "../../components/History";


export class AdminHistory extends Component {

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
                            <History  history={this.state.hist}/>
                        </div>

                    </div>
                </main>

                <footer> End of Page</footer>
            </div>
        )
    }
}

AdminHistory.contextType = GlobalContext;
