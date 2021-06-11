import NumberFormField from "../formsUtil/NumberFormField";
import axios from "axios";
import {alertRequestError_default} from "../utils";
import formatCurrency from "../pages/mainPage/currency";
import React from "react";
import {GlobalContext} from "../globalContext";
import ConditionalRender from "../ConditionalRender";

export default class HomeProduct extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            quantity: new NumberFormField(),
        };
        this.product = props.product;
    }

    onInputChange = e => {
        e.preventDefault();
        if (!this.state.quantity.trySetValueFromEvent(e)) {
            return;
        }
        this.setState({
            ...this.state
        });
    }

    onButtonClick = e => {
        if (!this.product._inCart) {
            this.onAddToCart(e);
        }
        else {
            this.onGoToCart(e);
        }
    }

    onAddToCart = e => {
        e.preventDefault();
        if (!this.state.quantity.validate()) {
            alert('Please fill in the desired quantity');
            return;
        }

        axios.post('/ShoppingCart/AddProduct', {
            username: this.context.username,
            productId: this.product.id,
            quantity: this.state.quantity.value,
        }).then(response => {
            this.product.cartQuantity = this.state.quantity.value;
            this.setState({
                quantity: new NumberFormField(),
            });
            this.props.addToCart(this.product);
        }, alertRequestError_default);
    }

    onGoToCart = e => {
        this.props.history.push('/ShoppingCart');
    }

    render() {
        return (
            <div className = "product">
                <a href={"#" + this.product.id}>
                    <p className= "productName">{this.product.name}</p>
                </a>
                <ConditionalRender condition={this.props.storeId == null}
                                   render={() => (<p className= "productName">{<text style={{fontWeight: "bold"}}>Store: </text>} {this.product.storeName}</p>)}
                />
                <p className= "productName"> {<text style={{fontWeight: "bold"}}>Quantity in store: </text>} {this.product.quantity}</p>
                {this.product._inCart ? (
                    <div style={{display: 'none'}} />
                ) : (
                    <input
                        type="number"
                        placeholder="Quantity"
                        style={{width: "8rem", height: "4rem", marginLeft:"10rem", marginBottom:"2rem", textAlign:"center"}}
                        required
                        value={this.state.quantity.value}
                        onChange={this.onInputChange}
                    />
                )}
                <div className="product-price">
                    <div>{formatCurrency(this.product.price)}</div>
                </div>
                <button onClick={this.onButtonClick} className="button primary">{this.product._inCart ? 'Go to shopping cart' : 'Add To Cart'}</button>
            </div>
        )
    }
}

HomeProduct.contextType = GlobalContext;
