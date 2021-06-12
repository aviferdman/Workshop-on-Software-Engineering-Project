import React from "react";
import BidRecordStore from "./BidRecordStore";
import {GlobalContext} from "../../../globalContext";

export default class BidRecordsStore extends React.Component {
    render() {
        if (this.props.bidRecords == null) {
            return null;
        }

        return (
            <div>
                <ul className = "simple-bids-ul">
                    {this.props.bidRecords.map((elem) => {
                        if (this.props.storeProductsMap != null) {
                            elem.product = this.props.storeProductsMap[elem.productId];
                        }

                        return (
                            <li  key={elem.id}>
                                <BidRecordStore bid={elem}
                                                storeId={this.props.storeId}
                                                myPermissions={this.props.myPermissions} />
                            </li>
                        );
                    })}
                </ul>
            </div>
        );
    }
}

BidRecordsStore.contextType = GlobalContext;