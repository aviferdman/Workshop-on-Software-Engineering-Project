import React, {Component} from 'react';
import CartProduct from "./CartProduct";

class CartProducts extends Component {
    render() {
        return (
            <div>
                <ul className = "products">
                    {this.props.products.map((product) => (
                        <li key={product.id}>
                            <CartProduct product={product}
                                         onEditProduct={this.props.onEditProduct}
                                         onRemoveProduct={this.props.onRemoveProduct} />
                        </li>
                    ))}
                </ul>
            </div>
        );
    }
}

export default CartProducts;