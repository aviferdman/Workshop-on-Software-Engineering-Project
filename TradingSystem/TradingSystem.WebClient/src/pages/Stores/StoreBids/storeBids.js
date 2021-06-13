import React from "react";
import './storeBids.css';
import * as api from "../../../api";
import {alertRequestError_default} from "../../../utils";
import BidRecordsStore from "./BidRecordsStore";
import * as util from "../../../utils";
import {GlobalContext} from "../../../globalContext";
import CheckboxFormField from "../../../formsUtil/CheckboxFormField";
import ConditionalRender from "../../../ConditionalRender";

export class StoreBids extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            bids: null,
            storeName: null,
            storeProductsMap: null,
            myPermissions: null,
            bidsPolicyCheckField: null,
            ready: false,
        };
        this.storeId = this.props.match.params.storeId;
    }

    async componentDidMount() {
        await Promise.all([
            this.fetchStoreProducts(),
            this.fetchBids(),
            this.fetchMyStorePermissions(),
            this.fetchIsBidsPolicyOn(),
        ]);

        this.setState({
            ready: true,
        });
    }

    async fetchBids() {
        await api.stores.bids.ofStore(this.context.username, this.storeId)
            .then(bids => {
                this.setState({
                    bids: bids,
                });
            }, e => {
                if (e.response != null && e.response.data === 'Bids are not supported in this store') {
                    return;
                }

                alertRequestError_default(e);
            })
    }

    async fetchStoreProducts() {
        await api.stores.infoWithProducts(this.storeId)
            .then(storeInfo => {
                this.setState({
                    storeName: storeInfo.name,
                    storeProductsMap: util.arrayToMap(storeInfo.products, p => p.id),
                });
            }, alertRequestError_default);
    }

    async fetchMyStorePermissions() {
        await api.stores.permissions.mine(this.context.username, this.storeId)
            .then(permissions => {
                this.setState({
                    myPermissions: {
                        role: permissions.role,
                        actions: util.arrayToHashset(permissions.permissions),
                    },
                });
            }, alertRequestError_default);
    }

    async fetchIsBidsPolicyOn() {
        await api.stores.bids.getBidPolicy({
            username: this.context.username,
            storeId: this.storeId,
        }).then(isBidsOn => {
            this.setState({
                bidsPolicyCheckField: new CheckboxFormField(isBidsOn),
            });
        }, alertRequestError_default);
    }

    onBidsPolicyCheckedChange = async e => {
        let field = this.state.bidsPolicyCheckField;
        let previousValue = field.getValue();
        if (!field.trySetValueFromEvent(e)) {
            return;
        }

        this.setState({
            bidsPolicyCheckField: field,
        });

        await api.stores.bids.changeBidPolicy({
            username: this.context.username,
            storeId: this.storeId,
            isAvailable: field.getValue(),
        }).then(() => {
        }, e => {
            field.setValue(previousValue);
            this.setState({
                bidsPolicyCheckField: field,
                bids: [],
            });
            alertRequestError_default(e);
        });
    }

    render() {
        if (!this.state.ready) {
            return null;
        }

        let bidsPolicyCheckField = this.state.bidsPolicyCheckField;
        return (
            <main>

                <div>

                    {/*top grid - simple discounts*/}
                    <div  className="store-bids-grid-simple">
                        <h2>  Store Bids </h2>
                        <div  className="col-grd-perm-bids">
                           <div className="text-props">
                               <label >Support Bids</label>
                           </div>

                           <div>
                               <ConditionalRender
                                   condition={bidsPolicyCheckField != null}
                                   render={() => (
                                       <input
                                           type="checkbox"
                                           className="input-props"
                                           value='bidsPolicy'
                                           checked={bidsPolicyCheckField.getInputValue()}
                                           onChange={this.onBidsPolicyCheckedChange}
                                       />
                                   )}
                               />
                           </div>
                        </div>
                        {bidsPolicyCheckField != null && bidsPolicyCheckField.getValue() ? (
                            <BidRecordsStore
                                bidRecords={this.state.bids}
                                storeProductsMap={this.state.storeProductsMap}
                                storeId={this.storeId}
                                myPermissions={this.state.myPermissions}
                            />
                        ) : (<h1 className='center-screen'>Bids are not supported in the store</h1>)}

                    </div>



                </div>

            </main>
        )
    }
}

StoreBids.contextType = GlobalContext;
