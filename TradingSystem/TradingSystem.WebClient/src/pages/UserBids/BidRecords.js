import React from "react";
import './userBids.css';
import {GlobalContext} from "../../globalContext";
import BidRecord from "./BidRecord";

export default class BidRecords extends React.Component {
    render() {
        if (this.props.bidRecords == null) {
            return null;
        }

        return (
            <div>
                <ul className = "simple-bids-ul">
                    {this.props.bidRecords.map((elem) => (
                        <li  key={elem.id}>
                            <BidRecord bid={elem} />
                        </li>
                    ))}
                </ul>
            </div>
        );
    }
}

BidRecords.contextType = GlobalContext;