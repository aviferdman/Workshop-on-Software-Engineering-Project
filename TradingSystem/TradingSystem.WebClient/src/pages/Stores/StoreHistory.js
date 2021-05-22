

import React, {Component} from 'react';
import * as HiIcons from "react-icons/hi";
import Navbar from "../../components/Navbar/Navbar";
import './StoreHistory.css';
import data from "../../data/historyData.json";
import History from "../../components/History";
import {Link} from "react-router-dom";
import {GlobalContext} from "../../globalContext";

export class StoreHistory extends Component {

    constructor(props) {
        super(props);
        this.state = {
            name: "",
            history: data.history
        };
    }
    render() {
        return (
            <div className="grid-container">
                <header className="header-container" >
                    <a href="/">E - commerce Application</a>
                    <div>
                        <h3>{this.context.isLoggedIn ? this.context.username : ''}</h3>
                    </div>


                    <Link
                        className="icons"
                        to={{
                            pathname: "/ShoppingCart"
                        }}
                    >
                        <HiIcons.HiShoppingCart />
                    </Link>


                    <Navbar></Navbar>

                </header>

                <main className="store-products-main-conatiner">

                    <div>
                        <History history={this.state.history} ></History>
                    </div>

                    <div className="bottom-row">


                    </div>

                </main>
                <footer> End of Store</footer>
            </div>
        )
    }
}

StoreHistory.contextType = GlobalContext;