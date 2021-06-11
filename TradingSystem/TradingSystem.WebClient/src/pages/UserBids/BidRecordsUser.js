import React from "react";
import {GlobalContext} from "../../globalContext";
import BidRecordUser from "./BidRecordUser";

export default class BidRecordsUser extends React.Component {
    render() {
        if (this.props.bidRecords == null) {
            return null;
        }

        return (
            <div>
                <ul className = "simple-bids-ul">
                    {this.props.bidRecords.map((elem) => (
                        <li  key={elem.id}>
                            <BidRecordUser bid={elem} />
                        </li>
                    ))}
                </ul>
            </div>
        );
    }
}

BidRecordsUser.contextType = GlobalContext;