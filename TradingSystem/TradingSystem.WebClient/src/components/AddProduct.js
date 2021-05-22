import React from "react";
import './AddProduct.css';
import axios from "axios";
import {alertRequestError_default} from "../utils";
import {GlobalContext} from "../globalContext";
import ProductFields from "../formsUtil/productFields";

class AddProduct extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            show: false,
            product: new ProductFields(),
        };

        this.onConfirmAddProduct = this.onConfirmAddProduct.bind(this);
    }

    showModal = () => {
        this.setState({ show: true });
    }

    hideModal = () => {
        this.setState({ show: false });
    }

    onInputChange = field => e => {
        e.preventDefault();
        if (!this.state.product.getField(field).trySetValueFromEvent(e)) {
            return;
        }
        this.setState({
           ...this.state
        });
    }

    onConfirmAddProduct(e) {
        e.preventDefault();
        if (!this.state.product.validate()) {
            alert('Please fill all fields');
            return;
        }

        axios.post('/Stores/AddProduct', {
            username: this.context.username,
            storeId: this.props.storeId,
            productDetails: this.state.product.valuesObject(),
        }).then(response => {
            this.setState({
                show: false
            });
        })
        .catch(alertRequestError_default);
    }

    render() {
        return (
            <main className="items">
                <Modal show={this.state.show} handleClose={this.hideModal} handleAdd={this.onConfirmAddProduct}>
                    <div className= "col-grd">

                        <div className="text-props">
                            <label>Name</label>
                        </div>

                        <div >
                            <input
                                type="text"
                                placeholder="Name"
                                className="input-props"
                                required
                                value={this.state.product.getValue('name')}
                                onChange={this.onInputChange('name')}
                            />
                        </div>
                    </div>

                    <div className= "col-grd">
                        <div className="text-props">
                            <label>Quantity</label>
                        </div>

                        <div >
                            <input
                                type="number"
                                placeholder="Quantity"
                                className="input-props"
                                required
                                value={this.state.product.getValue('quantity')}
                                onChange={this.onInputChange('quantity')}
                            />
                        </div>
                    </div>
                    <div className= "col-grd">
                        <div className="text-props">
                            <label>Price</label>
                        </div>

                        <div >
                            <input
                                type="number"
                                step="0.01"
                                placeholder="Price"
                                className="input-props"
                                required
                                value={this.state.product.getValue('price')}
                                onChange={this.onInputChange('price')}
                            />
                        </div>
                    </div>

                    <div className= "col-grd">
                        <div className="text-props">
                            <label>Weight</label>
                        </div>

                        <div >
                            <input
                                type="number"
                                step="0.01"
                                placeholder="Weight"
                                className="input-props"
                                required
                                value={this.state.product.getValue('weight')}
                                onChange={this.onInputChange('weight')}
                            />
                        </div>
                    </div>
                    <div className= "col-grd">
                        <div className="text-props">
                            <label>Category</label>
                        </div>

                        <div >
                            <input
                                type="text"
                                placeholder="Name"
                                className="input-props"
                                required
                                value={this.state.product.getValue('category')}
                                onChange={this.onInputChange('category')}
                            />
                        </div>
                    </div>

                </Modal>

                <button className= "store-products-button-view" onClick={this.showModal}> Add Product </button>
            </main>
        )
    }
}

const Modal = ({ handleClose, handleAdd, show, children }) => {
    const showHideClassName = show ? 'modal display-block' : 'modal display-none';

    return (
        <div className={showHideClassName}>
            <section className='modal-main'>

                <div className="lines-props">
                    <h2 className="center-text">Add Product</h2>
                    {children}
                </div>


                    <div className="modal-buttons">
                        <button className="modal-buttons-props" onClick={handleClose} > Close </button>
                        <button className="modal-buttons-props" onClick={handleAdd} > Add </button>
                    </div>


            </section>
        </div>
    );
};

AddProduct.contextType = GlobalContext;

export default AddProduct;
