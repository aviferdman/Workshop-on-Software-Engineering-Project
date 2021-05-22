import React, {Component} from 'react';
import formatCurrency from "../pages/mainPage/currency";
import * as AiIcons from "react-icons/ai";
import EditProduct from "./EditProduct";

class CartProducts extends Component {
    render() {
        return (
            <div>
                <ul className = "products">
                    {this.props.products.map((product) => (
                        <li key={product.id}>
                            <div className="control-buttons">
                                <div>
                                    <button className="exit-button">
                                        <AiIcons.AiOutlineClose />
                                    </button>
                                </div>

                            </div>
                            <div className = "product">
                                <a href={"#" + product.id}>
                                    <p className= "productName">{product.name}</p>
                                </a>
                                <p className= "productName"> store: {product.name}</p>
                                <input
                                    type="number"
                                    placeholder="Quantity"
                                    style={{width: "8rem", height: "4rem", marginLeft:"10rem", marginBottom:"2rem", textAlign:"center"}}
                                />

                                <div className="product-price">
                                    <div style={{ marginBottom:"0.5rem"}}>{formatCurrency(product.price)}</div>
                                </div>
                                <button onClick={() => this.props.addToCart(product)} className="button primary">Remove</button>
                            </div>
                        </li>
                    ))}
                </ul>
            </div>
        );
    }
}

export default CartProducts;