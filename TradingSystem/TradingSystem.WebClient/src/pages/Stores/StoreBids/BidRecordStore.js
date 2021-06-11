import React from "react";
import {GlobalContext} from "../../../globalContext";

export default class BidRecordStore extends React.Component {
    render() {
        let bid = this.props.bid;
        let product = bid.product;

        return (
            <div className = "simple-bids-li-div">
                <p className= "bidName">{<text style={{fontWeight: "bold"}}>User: </text>} {bid.username}  </p>
                <p className= "bidName"> {<text style={{fontWeight: "bold"}}>Product: </text>} {product.name}  </p>
                <p className= "bidName"> {<text style={{fontWeight: "bold"}}>Current Offer:</text>} {bid.price}  </p>
                <p className= "bidName">{<text style={{fontWeight: "bold"}}>Status:</text>} {
                    bid.status === "Approved" ? (<label style={{color: "green"}}>Approved</label>):
                    bid.status === "Negotiation" ? (<label style={{color: "orange"}}>Negotiation</label>):
                        (<label style={{color: "red"}}>Declined</label>)}
                </p>
                <p className= "bidName"> {<text style={{fontWeight: "bold"}}>Action: </text>}
                    {
                        <select >
                            {/*if Approve is chosen a pop-up should appear to warn about Approving the offer permanently */}
                            <option value="Approve">Approved</option>
                            {/*if Negotiate is chosen a pop-up should appear to update the current price */}
                            <option value="Negotiate">Negotiate</option>
                            {/*if Decline is chosen a pop-up should appear to warn about declining the offer permanently */}
                            <option value="Decline">Decline</option>

                        </select>
                    }

                </p>

                <button className="button primary" style={{margin: "2rem"}}>Apply Action</button>

            </div>
        );
    }
}

BidRecordStore.contextType = GlobalContext;