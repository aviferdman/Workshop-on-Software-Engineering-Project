import React, {Component} from 'react';
import * as HiIcons from "react-icons/hi";
import Navbar from "../../components/Navbar/Navbar";
import './StoreHistory.css';
import data from "../../data/historyData.json";
import History from "../../components/History";

class StoreHistory extends Component {

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
                <header className="header-container">
                    <a href="/">E - commerce Application</a>
                    <div/>
                    <button className="icons">
                        <HiIcons.HiShoppingCart />
                    </button>
                    <Navbar/>
                </header>

                <main className="store-products-main-conatiner">

                    <div>
                        <History history={this.state.history} />
                    </div>

                    <div className="bottom-row">


                    </div>

                </main>
                <footer> End of Store</footer>
            </div>
        )
    }
}

export default StoreHistory;