import React, {Component} from 'react';
import formatCurrency from "../pages/mainPage/currency";
import * as AiIcons from "react-icons/ai";
import EditProduct from "./EditProduct";
import axios from "axios";
import {alertRequestError_default} from "../utils";
import {GlobalContext} from "../globalContext";

class Products extends Component {
    onRemove = product => e => {
        e.preventDefault();

        axios.post('/Stores/RemoveProduct', {
            username: this.context.username,
            storeId: this.props.storeId,
            productId: product.id,
        }).then(response => {
            this.props.onProductRemoved(product);
        }).catch(alertRequestError_default);
    }

    renderControlButtonsElement = product => {
        let controlButtonsElement;

        // whether this component was called from within a store
        if (this.props.storeId) {
            controlButtonsElement = (
                <div className="control-buttons">
                    <div>
                        <button className="exit-button" onClick={this.onRemove(product)}>
                            <AiIcons.AiOutlineClose />
                        </button>
                    </div>

                    <div>
                        <EditProduct storeId={this.props.storeId} productId={product.id} onProductEdited={this.props.onProductEdited} />
                    </div>


                </div>
            );
        }
        else {
            controlButtonsElement = <div/>
        }

        return controlButtonsElement;
    }

    render() {
        return (
            <div>
                <ul className = "products">
                {this.props.products.map((product) => (
                    <li key={product.id}>
                        {this.renderControlButtonsElement(product)}
                        <div className = "product">
                            <a>
                                <p className= "productName">{product.name}</p>
                            </a>
                            <div className="product-price">
                                <div>{formatCurrency(product.price)}</div>
                            </div>
                            {/*
                            TODO: ask gil
                            <button onClick={() => this.props.addToCart(product)} className="button primary">Add To Cart</button>
                             */}
                        </div>
                    </li>
                    ))}
                </ul>
            </div>
        );
    }
}

Products.contextType = GlobalContext;

export default Products;