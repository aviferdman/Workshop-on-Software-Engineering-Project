import React, {Component} from 'react';
import Cart from "../../components/Cart";

class ShoppingCart extends Component {

    constructor(props) {
        super(props);
        this.state = {
            cartItems: []
        };
    }

    removeFromCart = (product) => {
        const cartItems = this.state.cartItems.slice();
        this.setState({
            cartItems: cartItems.filter(x => x.id !== product.id),
        });
    };

    render() {
        return (
            <div>
                <Cart cartItems={this.state.cartItems} removeFromCart={this.removeFromCart} />
            </div>
        );
    }
}

export default ShoppingCart;