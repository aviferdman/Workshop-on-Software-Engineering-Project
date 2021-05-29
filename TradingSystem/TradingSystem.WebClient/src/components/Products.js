import React, {Component} from 'react';
import formatCurrency from "../pages/mainPage/currency";
import * as AiIcons from "react-icons/ai";
import EditProduct from "./EditProduct";
import axios from "axios";
import {alertRequestError_default} from "../utils";
import {GlobalContext} from "../globalContext";
import StoreRestrictedComponentCustom from "./StoreRestrictedComponentCustom";
import * as api from "../api";

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
            let counter = 0;
            controlButtonsElement = (
                <div className="control-buttons">
                    <div>
                        <StoreRestrictedComponentCustom
                            permissions={this.props.myPermissions}
                            allowedActions={[api.data.stores.permissions.removeProduct,]}
                            render={() => {
                                counter++;
                                return (
                                    <button className="exit-button" onClick={this.onRemove(product)}>
                                        <AiIcons.AiOutlineClose />
                                    </button>
                                )}
                            } />
                    </div>

                    <div>
                        <StoreRestrictedComponentCustom
                            permissions={this.props.myPermissions}
                            allowedActions={[api.data.stores.permissions.editProduct,]}
                            render={() => {
                                counter++;
                                return (
                                    <EditProduct storeId={this.props.storeId} product={product} onProductEdited={this.props.onProductEdited} />
                                )}
                            } />
                    </div>

                </div>
            );
            if (counter === 0) {
                controlButtonsElement = (
                    <div className="control-buttons margin-no-controls">{controlButtonsElement.props.children}</div>
                );
            }
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
                        <div className = "store-product">
                            <a>
                                <p className= "productName">{product.name}</p>
                            </a>
                            <div className="product-price">
                                <div>{formatCurrency(product.price)}</div>
                            </div>
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