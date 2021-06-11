import React from "react";
import {GlobalContext} from "../../../globalContext";
import * as api from '../../../api';
import {alertRequestError_default} from "../../../utils";

export default class BidRecordStore extends React.Component {
    applyAction() {
    }

    render() {
        let bid = this.props.bid;
        let product = bid.product;
        let isInNegotiation = bid.status === api.data.bids.customerNegotiate || api.data.bids.ownerNegotiate;

        return (
            <div className="simple-bids-li-div">
                <p className="bidName">{<text style={{ fontWeight: "bold" }}>User: </text>} {bid.username}  </p>
                <p className="bidName"> {<text style={{ fontWeight: "bold" }}>Product: </text>} {product.name}  </p>
                <p className= "bidName"> {<text style={{fontWeight: "bold"}}>Current Offer:</text>} {bid.price}  </p>
                <p className= "bidName">{<text style={{fontWeight: "bold"}}>Status:</text>} {
                    bid.status === api.data.bids.approved ? (<label style={{color: "green"}}>Approved</label>) :
                    bid.status === api.data.bids.ownerNegotiate ? (<label style={{color: "orange"}}>Negotiation (waiting on customer)</label>) :
                    bid.status === api.data.bids.customerNegotiate && bid.approvedByMe ? (<label style={{color: "orange"}}>Negotiation (waiting on staff)</label>) :
                    bid.status === api.data.bids.customerNegotiate ? (<label style={{color: "orange"}}>Negotiation (waiting on you)</label>) :
                    bid.status === api.data.bids.declined ? (<label style={{color: "red"}}>Declined</label>) :
                    null
                }
                </p>

                <p className="bidName" style={{ visibility: isInNegotiation ? 'visible' : 'hidden' }}>
                    <text style={{fontWeight: "bold"}}>Action: </text>
                    <select>
                        {/*if Approve is chosen a pop-up should appear to warn about Approving the offer permanently */}
                        {bid.status === api.data.bids.customerNegotiate ? (<option value="Approve">Approve</option>) : null}
                        {/*if Negotiate is chosen a pop-up should appear to update the current price */}
                        <option value="Negotiate">Negotiate</option>
                        {/*if Decline is chosen a pop-up should appear to warn about declining the offer permanently */}
                        <option value="Decline">Decline</option>
                    </select>
                </p>

                <button className="button primary" style={{
                    margin: "2rem",
                    visibility: isInNegotiation ? 'visible' : 'hidden'
                }}>Apply Action</button>

            </div>
        );
    }
}

BidRecordStore.contextType = GlobalContext;
