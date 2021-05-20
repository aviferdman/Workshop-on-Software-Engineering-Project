

import React, {Component} from 'react';
import * as HiIcons from "react-icons/hi";
import Navbar from "../../components/Navbar/Navbar";
import './StoreHistory.css';
import data from "../../data/staffData.json";
import AddProduct from "../../components/AddProduct";
import Users from "../../components/Users";
import AddManager from "../../components/AddManager";
import AddOwner from "../../components/AddOwner";
import SetPermission from "../../components/SetPermission";

class StoreHistory extends Component {

    constructor(props) {
        super(props);
        this.state = {
            name: "",
            staff: data.staff
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

                <main className="store-products-main-conatiner">

                    <div>
                        <Users staff={this.state.staff} ></Users>
                    </div>

                    <div className="bottom-row">
                        <div>
                            <AddManager></AddManager>
                        </div>

                        <div>
                            <AddOwner></AddOwner>
                        </div>

                        <div>
                            <SetPermission></SetPermission>
                        </div>


                    </div>

                </main>
                <footer> End of Store</footer>
            </div>
        )
    }
}

export default StoreHistory;