import React from "react";
import './searchStore.css';
import {GlobalContext} from "../../globalContext";
import Header from "../../header";
import SearchBar from "../../components/searchBar";
import Data from "../../data/StoresData.json"
import StoreListUserView from "./StoreListUserView";

export class searchStore extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            name: "",
            stores: Data.stores
        };
    }

    render() {
        return (
            <div className="grid-container">
                <Header />

                <main>
                    <div className="search-store-grid">
                        <div className="search-store-element">
                            <SearchBar />
                            {" "}
                        </div>

                        <div className="stores-view-flex">
                            <StoreListUserView  stores={this.state.stores}/>
                        </div>

                    </div>

                </main>

                <footer> End of Stores</footer>
            </div>
        )
    }
}

searchStore.contextType = GlobalContext;
