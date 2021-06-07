import * as AiIcons from "react-icons/ai";
import {Link} from "react-router-dom";
import formatCurrency from "../pages/mainPage/currency";
import React from "react";
import NumberFormField from "../formsUtil/NumberFormField";
import axios from "axios";
import {alertRequestError_default} from "../utils";
import {GlobalContext} from "../globalContext";

export default class CartProduct extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            quantity: new NumberFormField(props.product.quantity),
        };
    }

    componentDidMount() {
        this.timeoutId = null;
    }

    componentWillUnmount() {
        this.clearTimeout();
    }

    clearTimeout = () => {
        if (this.timeoutId != null) {
            clearTimeout(this.timeoutId);
            this.timeoutId = null;
        }
    }

    setTimeout = f => {
        this.timeoutId = setTimeout(f, 500);
    }

    onQuantityChange = e => {
        e.preventDefault();
        this.clearTimeout();
        if (!this.state.quantity.trySetValueFromEvent(e)) {
            return;
        }
        this.setState({
            ...this.state
        });
        this.setTimeout(async () => {
            this.timeoutId = null;
            await this.sendUpdateCartProductRequest();
        });
    }

    sendUpdateCartProductRequest = async () => {
        await axios.post('/ShoppingCart/EditProduct', {
            username: this.context.username,
            productId: this.props.product.id,
            quantity: this.state.quantity.value,
        }).then(response => {
            this.props.product.quantity = this.state.quantity.value;
            this.props.onEditProduct(this.props.product);
        }, alertRequestError_default);
    }

    onRemoveClick = async () => {
        await axios.post('/ShoppingCart/RemoveProduct', {
            username: this.context.username,
            productId: this.props.product.id,
        }).then(response => {
            this.props.onRemoveProduct(this.props.product);
        }, alertRequestError_default);
    }

    render() {
        return (
            <div>
                <div className="control-buttons">
                    <div>
                        <button className="exit-button" onClick={this.onRemoveClick}>
                            <AiIcons.AiOutlineClose />
                        </button>
                    </div>
                </div>

                <div className = "product">
                    <a href={"#" + this.props.product.id}>
                        <p className= "productName">{this.props.product.name}</p>
                    </a>
                    <p className= "productName">
                        store:<span> </span>
                        <Link to={{ pathname: `/store/${this.props.product.store.id}` }}>
                            {this.props.product.store.name}
                        </Link>
                    </p>
                    <input
                        type="number"
                        placeholder="Quantity"
                        style={{width: "8rem", height: "4rem", marginLeft:"10rem", marginBottom:"2rem", textAlign:"center"}}
                        required
                        value={this.state.quantity.value}
                        onChange={this.onQuantityChange}
                    />

                    <div className="product-price">
                        <div style={{ marginBottom:"0.5rem"}}>{formatCurrency(this.props.product.price)}</div>
                    </div>
                    <button onClick={this.onRemoveClick} className="button primary">Remove</button>
                </div>
            </div>
        )
    }
}

CartProduct.contextType = GlobalContext;
