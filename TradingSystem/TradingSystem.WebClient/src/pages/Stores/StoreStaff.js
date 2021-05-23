import React, {Component} from 'react';
import './StoreStaff.css';
import data from "../../data/staffData.json";
import Users from "../../components/Users";
import AddManager from "../../components/AddManager";
import AddOwner from "../../components/AddOwner";
import SetPermission from "../../components/SetPermission";
import {GlobalContext} from "../../globalContext";
import Header from "../../header";

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
                <Header />

                <main className="store-products-main-conatiner-staff">

                    <div>
                        <Users staff={this.state.staff} />
                    </div>

                    <div className="bottom-row-staff">
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

StoreStaff.contextType = GlobalContext;