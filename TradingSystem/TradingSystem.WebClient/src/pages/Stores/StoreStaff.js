import React, {Component} from 'react';
import * as HiIcons from "react-icons/hi";
import Navbar from "../../components/Navbar/Navbar";
import './StoreStaff.css';
import data from "../../data/staffData.json";
import Users from "../../components/Users";
import AddManager from "../../components/AddManager";
import AddOwner from "../../components/AddOwner";
import SetPermission from "../../components/SetPermission";

class StoreStaff extends Component {

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
                    <div/>
                    <button className="icons">
                        <HiIcons.HiShoppingCart />
                    </button>
                    <Navbar/>
                </header>

                <main className="store-products-main-conatiner">

                    <div>
                        <Users staff={this.state.staff} />
                    </div>

                    <div className="bottom-row">
                        <div>
                            <AddManager/>
                        </div>

                        <div>
                            <AddOwner/>
                        </div>

                        <div>
                            <SetPermission/>
                        </div>


                    </div>

                </main>
                <footer> End of Store</footer>
            </div>
        )
    }
}

export default StoreStaff;