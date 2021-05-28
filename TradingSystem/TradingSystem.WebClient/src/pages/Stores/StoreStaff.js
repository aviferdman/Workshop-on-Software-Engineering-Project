import React, {Component} from 'react';
import './StoreStaff.css';
import data from "../../data/staffData.json";
import Users from "../../components/Users";
import AddManager from "../../components/AddManager";
import AddOwner from "../../components/AddOwner";
import SetPermission from "../../components/SetPermission";
import {GlobalContext} from "../../globalContext";

export class StoreStaff extends Component {
    constructor(props) {
        super(props);
        this.state = {
            name: "",
            staff: data.staff
        };
        this.storeId = this.props.match.params.storeId;
    }

    render() {
        return (
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
        )
    }
}

StoreStaff.contextType = GlobalContext;