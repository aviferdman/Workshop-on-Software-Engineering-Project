import React from "react";
import './Policy.css';
import {GlobalContext} from "../../../globalContext";
import Header from "../../../header";
import DataSimple from "../../../data/policyData.json"
import AddPolicy from "./AddPolicy";
import PolicyRecord from "./PolicyRecord";


export class Policy extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            name: "",
            policy: DataSimple.policies,
        };
    }

    render() {
        return (
            <main>

                <div className="policy-grid">

                    {/*top grid - simple discounts*/}
                    <div  className="policy-grid-simple">
                        <h2> Store Policy </h2>
                        <PolicyRecord policyRecords={this.state.policy}  />
                    </div>

                    {/*bottom grid - buttons*/}
                    <div className="policy-grid-button">
                        <div className="center-btn-st">
                            <AddPolicy  />
                        </div>
                    </div>

                </div>

            </main>
        )
    }
}

Policy.contextType = GlobalContext;
