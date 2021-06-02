import formatCurrency from "../../pages/mainPage/currency";
import React from "react";
import {GlobalContext} from "../../globalContext";

export default class HistoryProduct extends React.Component {
    constructor(props) {
        super(props);
        this.product = props.product;
    }

    render() {
        return (
            <div className = "product">
                <a href={"#" + this.product.id}>
                    <p className= "productName">{this.product.name}</p>
                </a>
                <p className= "productName"> store: {this.product.storeName}</p>

                <p className= "productName"> quantity: {this.product.quantity}</p>

                <div className="product-price">
                    <h3 className= "productName"> Unit Price: </h3> <div>{formatCurrency(this.product.price)}</div>
                </div>
            </div>
        )
    }
}

HistoryProduct.contextType = GlobalContext;
