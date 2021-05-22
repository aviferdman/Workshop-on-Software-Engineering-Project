import React, {Component} from 'react';
import * as HiIcons from "react-icons/hi";
import Navbar from "../../components/Navbar/Navbar";
import './StoreStaff.css';
import data from "../../data/staffData.json";
import Users from "../../components/Users";
import AddManager from "../../components/AddManager";
import AddOwner from "../../components/AddOwner";
import SetPermission from "../../components/SetPermission";
import {Link} from "react-router-dom";
import {GlobalContext} from "../../globalContext";

export class StoreStaff extends Component {

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

                <main className="store-products-main-conatiner-staff">

                    <div>
                        <Users staff={this.state.staff} ></Users>
                    </div>

                    <div className="bottom-row-staff">
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

StoreStaff.contextType = GlobalContext;