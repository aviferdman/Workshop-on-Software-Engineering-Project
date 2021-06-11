import React from "react";
import './Policy.css';
import {GlobalContext} from "../../../globalContext";
import AddPolicy from "./AddPolicy";
import PolicyRecord from "./PolicyRecord";
import * as api from "../../../api";
import {alertRequestError_default} from "../../../utils";
import * as util from "../../../utils";


export class Policy extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            policies: null,
            storeProducts: null,
            storeProductsMap: null,
            nextId: 0,
        };
        this.storeId = this.props.match.params.storeId;
    }

    async componentDidMount() {
        let promise_storeProducts = this.fetchStoreProducts();
        let promise_policies = this.fetchPolicies();
        await Promise.all([promise_storeProducts, promise_policies]);
    }

    async fetchStoreProducts() {
        await api.stores.infoWithProducts(this.storeId)
            .then(storeInfo => {
                this.setState({
                    storeProducts: storeInfo.products,
                    storeProductsMap: util.arrayToMap(storeInfo.products, p => p.id),
                });
            }, alertRequestError_default);
    }

    async fetchPolicies() {
        await api.stores.policies.fetchAll(this.storeId)
            .then(policies => {
                this.setState({
                    policies: policies,
                });
            }, alertRequestError_default);
    }

    onPolicyAdd = policy => {
        let id = this.state.nextId;
        policy.id = id;
        this.state.policies.push(policy);
        this.setState({
            nextId: id + 1
        });
    }

    render() {
        return (
            <main>

                <div className="policy-grid">

                    {/*top grid - simple discounts*/}
                    <div  className="policy-grid-simple">
                        <h2> Store Policy </h2>
                        <PolicyRecord policyRecords={this.state.policies} storeProductsMap={this.state.storeProductsMap} />
                    </div>

                    {/*bottom grid - buttons*/}
                    <div className="policy-grid-button">
                        <div className="center-btn-st">
                            <AddPolicy storeId={this.storeId} storeProducts={this.state.storeProducts} onSuccess={this.onPolicyAdd} />
                        </div>
                        <div className="center-btn-nd">
                            <button className="store-products-button-view"> Remove All</button>
                        </div>
                    </div>

                </div>

            </main>
        )
    }
}

Policy.contextType = GlobalContext;
