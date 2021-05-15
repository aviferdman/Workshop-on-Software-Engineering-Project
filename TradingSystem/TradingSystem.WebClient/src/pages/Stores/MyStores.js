import React from "react";
import './MyStores.css';
import Navbar from "../../components/Navbar/Navbar";
import * as HiIcons from "react-icons/hi";
import Store from "./StoreList"
import axios from "axios";
import {GlobalContext} from "../../globalContext";

export class MyStores extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            name: "",
            stores: []
        };
    }

    async componentDidMount() {
        try {
            let response = await axios.post('/Stores/MyStores', '"' + this.context.username + '"', {
                headers: {
                    'content-type': 'application/json'
                }
            });
            this.setState({
                stores: response.data
            });
        }
        catch (e) {
            console.error("search error occurred: ", e);
        }
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

MyStores.contextType = GlobalContext;
