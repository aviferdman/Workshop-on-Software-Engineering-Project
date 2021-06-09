import React from "react";
import './Policy.css';
import {GlobalContext} from "../../../globalContext";
import AddPolicy from "./AddPolicy";
import PolicyRecord from "./PolicyRecord";


export class Policy extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            policies: [],
        };
    }

    onPolicyAdd = policy => {
        this.state.policies.push(policy);
        this.setState({
            ...this.state
        });
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
                            <AddPolicy storeId={this.storeId} onSuccess={this.onPolicyAdd} />
                        </div>
                    </div>

                </div>

            </main>
        )
    }
}

Policy.contextType = GlobalContext;
