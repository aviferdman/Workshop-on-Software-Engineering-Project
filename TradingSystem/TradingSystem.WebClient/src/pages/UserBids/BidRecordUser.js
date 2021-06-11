import React from "react";
import {GlobalContext} from "../../globalContext";
import * as api from '../../api'
import NumberFormField from "../../formsUtil/NumberFormField";
import {alertRequestError_default} from "../../utils";
import ConditionalRender from "../../ConditionalRender";

export default class BidRecordUser extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            inCart: false,
            quantity: new NumberFormField(),
        };
    }

    applyAction() {

    }

    async addToCart() {
        await api.shoppingCart.addProduct({
            username: this.context.username,
            productId: this.props.bid.productId,
            quantity: this.state.quantity.getValue(),
        }).then(response => {
            this.setState({
                inCart: true,
                quantity: new NumberFormField(),
            });
        }, alertRequestError_default);
    }

    goToShoppingCart() {
        this.props.history.push('/ShoppingCart');
    }

    onQuantityChange = e => {
        if (!this.state.price.trySetValueFromEvent(e)) {
            return;
        }

        this.setState({
            ...this.state
        });
    }

    render() {
        let bid = this.props.bid;
        let isInNegotiation = bid.status === api.data.bids.customerNegotiate || api.data.bids.ownerNegotiate;

        return (
            <div className = "simple-bids-li-div">
                <p className= "bidName">{<text style={{fontWeight: "bold"}}>Store: </text>} {bid.storeName} </p>
                <p className= "bidName"> {<text style={{fontWeight: "bold"}}>Product: </text>} {bid.productName}  </p>
                <p className= "bidName"> {<text style={{fontWeight: "bold"}}>Current Offer:</text>} {bid.price}  </p>
                <p className= "bidName">{<text style={{fontWeight: "bold"}}>Status:</text>} {
                    bid.status === api.data.bids.approved ? (<label style={{color: "green"}}>Approved</label>) :
                    bid.status === api.data.bids.ownerNegotiate ? (<label style={{color: "orange"}}>Negotiation (your turn)</label>) :
                    bid.status === api.data.bids.customerNegotiate ? (<label style={{color: "orange"}}>Negotiation (waiting on store)</label>) :
                    bid.status === api.data.bids.declined ? (<label style={{color: "red"}}>Declined</label>) :
                    null
                }
                </p>

                <ConditionalRender
                    condition={!this.state.inCart && isInNegotiation}
                    render={() => (
                        <p className= "bidName">
                            <text style={{fontWeight: "bold"}}>Action: </text>
                            <select>
                                {/*if Approve is chosen a pop-up should appear to warn about Approving the offer permanently */}
                                {bid.status === api.data.bids.ownerNegotiate ? (<option value="Approve">Approve</option>) : null}
                                {/*if Negotiate is chosen a pop-up should appear to update the current price */}
                                <option value="Negotiate">Negotiate</option>
                                {/*if Decline is chosen a pop-up should appear to warn about declining the offer permanently */}
                                <option value="Decline">{bid.status === api.data.bids.customerNegotiate ? 'Cancel bid' : 'Decline'}</option>
                            </select>
                        </p>
                    )}
                />

                {!this.state.inCart && isInNegotiation ?
                    (<button className="button primary" style={{margin: "2rem"}}>Apply Action</button>) : null
                }
                {!this.state.inCart && bid.status === api.data.bids.approved ? (
                    <input
                        type="number"
                        placeholder="Quantity"
                        style={{width: "8rem", height: "4rem", marginLeft:"10rem", marginBottom:"2rem", textAlign:"center"}}
                        required
                        value={this.state.quantity.getInputValue()}
                        onChange={this.onQuantityChange}
                    />
                ) : null}
                {!this.state.inCart && bid.status === api.data.bids.approved ? (<button className="button primary" style={{margin: "2rem"}} onClick={this.addToCart}>Add To Cart</button>) : null}
                {this.state.inCart ? (<button className="button primary" style={{margin: "2rem"}} onClick={this.goToShoppingCart}>Go to shopping cart</button>) : null}

            </div>
        );
    }
}

BidRecordUser.contextType = GlobalContext;