import React from "react";
import './MyStores.css';
import Navbar from "../../components/Navbar/Navbar";
import * as HiIcons from "react-icons/hi";
import data from "./StoresData.json";
import Store from "./StoreList"

export class MyStores extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            name: "",
            stores: data.stores
        };
    }

    render() {
            return (
                <div className="grid-container">
                    <header className="header-container">
                        <a href="/">E - commerce Application</a>
                        <div></div>
                        <button className="icons">
                            <HiIcons.HiShoppingCart />
                        </button>
                        <Navbar></Navbar>
                    </header>

                    <main>
                        <Store stores={this.state.stores}>  </Store>

                    </main>
                    <footer> End of Stores</footer>
                </div>
            )
        }
    }

