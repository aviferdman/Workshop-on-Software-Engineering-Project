import React, {Component} from 'react';
import formatCurrency from "../pages/mainPage/currency";
import NumberFormField from "../formsUtil/NumberFormField";
import axios from "axios";
import {alertRequestError_default} from "../utils";
import {GlobalContext} from "../globalContext";

class HomeProducts extends Component {
    constructor(props) {
        super(props);
        this.state = {
            quantity: new NumberFormField(),
        };
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

    onAddToCartButtonClick = product => e => {
        e.preventDefault();
        if (!this.state.quantity.validate()) {
            alert('Please fill in the desired quantity');
            return;
        }

        axios.post('/ShoppingCart/AddProduct', {
            username: this.context.username,
            productId: product.id,
            quantity: this.state.quantity.value,
        }).then(response => {
            this.setState({
                quantity: new NumberFormField(),
            });
            this.props.addToCart(product);
        }, alertRequestError_default);
    }

    render() {
        return (
            <div>
                <ul className = "products">
                    {this.props.products.map((product) => (
                        <li key={product.id}>
                            <div className = "product">
                                <a href={"#" + product.id}>
                                    <p className= "productName">{product.name}</p>
                                </a>
                                <p className= "productName"> store: {product.name}</p>
                                <input
                                    type="number"
                                    placeholder="Quantity"
                                    style={{width: "8rem", height: "4rem", marginLeft:"10rem", marginBottom:"2rem", textAlign:"center"}}
                                    required
                                    value={this.state.quantity.value}
                                    onChange={this.onInputChange}
                                />
                                <div className="product-price">
                                    <div>{formatCurrency(product.price)}</div>
                                </div>
                                <button onClick={this.onAddToCartButtonClick(product)} className="button primary">Add To Cart</button>
                            </div>
                        </li>
                    ))}
                </ul>
            </div>
        );
    }
}

HomeProducts.contextType = GlobalContext;
export default HomeProducts;