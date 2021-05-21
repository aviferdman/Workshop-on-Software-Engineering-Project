import React, {Component} from 'react';
import formatCurrency from "../pages/mainPage/currency";
import * as AiIcons from "react-icons/ai";
import EditProduct from "./EditProduct";

class HomeProducts extends Component {
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
                                <div className="product-price">
                                    <div>{formatCurrency(product.price)}</div>
                                </div>
                                <button onClick={() => this.props.addToCart(product)} className="button primary">Add To Cart</button>
                            </div>
                        </li>
                    ))}
                </ul>
            </div>
        );
    }
}

export default HomeProducts;