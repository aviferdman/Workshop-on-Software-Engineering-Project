import React, {Component} from 'react';
import formatCurrency from "../pages/mainPage/currency";

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
                                <p className= "productName"> store: {product.name}</p>
                                <input
                                    type="number"
                                    placeholder="Quantity"
                                    style={{width: "8rem", height: "4rem", marginLeft:"10rem", marginBottom:"2rem", textAlign:"center"}}
                                />
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