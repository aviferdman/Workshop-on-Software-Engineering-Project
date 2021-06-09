import React from "react";
import './searchStore.css';
import {GlobalContext} from "../../globalContext";
import Header from "../../header";
import SearchBar from "../../components/searchBar";
import StoreListUserView from "./StoreListUserView";
import * as api from "../../api";

export class searchStore extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            name: "",
            stores: null
        };
    }


    onSearchInputChange = e => {
        this.setState({
            name: e.target.value
        });
    };

    onSearch = async e => {
        if (!this.state.name) {
            return;
        }
        await api.stores.search(this.state.name)
            .then(stores => {
                this.setState({
                    stores: stores,
                });
            });
    }


    render() {
        return (
            <div className="grid-container">
                <Header />

                <main>
                    <div className="search-store-grid">
                        <div className="search-store-element">
                            <SearchBar value={this.state.name} onChange={this.onSearchInputChange}
                                onSearch={this.onSearch} />
                            {" "}
                        </div>

                        <div className="stores-view-flex">
                            {this.state.stores === null ? null : (
                                <StoreListUserView stores={this.state.stores} history={this.props.history} />
                            )}
                        </div>

                    </div>

                </main>

                <footer> End of Stores</footer>
            </div>
        )
    }
}

searchStore.contextType = GlobalContext;