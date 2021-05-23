import React, {Component} from 'react';
import "./Cart.css"
import formatCurrency from "../pages/mainPage/currency";

class Cart extends Component {
    render() {
        const {cartItems} = this.props;
        return (
            <div>
                {cartItems.length === 0 ?
                    (<div className="cart cart-header"> Cart is empty </div>) :
                    ( <div className="cart cart-header"> You have {cartItems.length} item in the cart {" "} </div>)
                }

                <div>
                    <div className="cart">
                        <ul className="cart-items">
                            {cartItems.map((item) =>
                            <li key={item.id}>
                                <div>
                                    <div>{item.name}</div>
                                    <div>
                                        {formatCurrency(item.price)} x {item.count} {" "}
                                        <button className="button"
                                            onClick={() => this.props.removeFromCart(item)}> Remove
                                        </button>
                                    </div>
                                </div>
                            </li>
                            )}
                        </ul>
                    </div>
                    {cartItems.length !== 0 && (
                        <div className="cart">
                            <div className="total">
                                <div>
                                    Total: {" "}
                                    {/* TODO: fetch total price form server */}
                                    {formatCurrency(cartItems.reduce((a,c) => a + (c.price * c.count), 0))}
                                </div>
                                <button className="button primary" onClick={this.props.onProceed}> Proceed To Purchase</button>
                            </div>
                        </div>
                    )}

                </div>
            </div>
        );
    }
}

export default Cart;