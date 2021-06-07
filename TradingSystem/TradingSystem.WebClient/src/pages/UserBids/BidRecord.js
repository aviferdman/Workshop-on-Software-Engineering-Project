import React from "react";
import './userBids.css';
import {GlobalContext} from "../../globalContext";



export default class BidRecord extends React.Component {
    constructor(props) {
        super(props);
        this.action = "Approve"

    }

    render() {
        return (
            <div>
                <ul className = "simple-bids-ul">
                    {this.props.bidRecords.map((elem) => (
                        <li  key={elem.id}>
                            <div className = "simple-bids-li-div">
                                <p className= "bidName">{<text style={{fontWeight: "bold"}}>User: </text>} {elem.user}  </p>
                                <p className= "bidName">{<text style={{fontWeight: "bold"}}>Store: </text>} {elem.store} </p>
                                <p className= "bidName"> {<text style={{fontWeight: "bold"}}>Product: </text>} {elem.product}  </p>
                                <p className= "bidName"> {<text style={{fontWeight: "bold"}}>Current Offer</text>} {elem.currentOffer}  </p>
                                <p className= "bidName">{<text style={{fontWeight: "bold"}}>Status</text>}: {
                                    elem.status === "Approved" ? (<label style={{color: "green"}}>Approved</label>):
                                    elem.status === "Negotiation" ? (<label style={{color: "orange"}}>Negotiation</label>):
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

                                <button className="button primary" style={{margin: "2rem"}}>{'Add To Cart'}</button>

                            </div>
                        </li>
                    ))}
                </ul>
            </div>
        );
    }
}

BidRecord.contextType = GlobalContext;