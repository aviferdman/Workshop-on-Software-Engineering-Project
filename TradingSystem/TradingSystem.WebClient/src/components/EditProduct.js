import React from "react";
import './AddProduct.css';
import * as AiIcons from "react-icons/ai";
import {GlobalContext} from "../globalContext";
import ProductFields from "../formsUtil/productFields";
import axios from "axios";
import {alertRequestError_default} from "../utils";

class EditProduct extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            show: false,
            product: new ProductFields(),
        };

        let fields = this.state.product.fields;
        let product = this.props.product;
        fields.name.value = product.name;
        fields.quantity.value = product.quantity;
        fields.price.value = product.price;
        fields.category.value = product.category;
        fields.weight.value = product.weight;

        this.onConfirm = this.onConfirm.bind(this);
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

    onConfirm(e) {
        e.preventDefault();
        if (!this.state.product.validate()) {
            alert('Please fill all fields');
            return;
        }

        let product = this.state.product.valuesObject();
        axios.post('/Stores/EditProduct', {
            username: this.context.username,
            storeId: this.props.storeId,
            productId: this.props.product.id,
            productDetails: product,
        }).then(response => {
            this.setState({
                show: false,
            });
            product.id = this.props.product.id;
            this.props.onProductEdited(product);
        }, alertRequestError_default);
    }

    render() {
        return (
            <main className="items-edit">
                <Modal show={this.state.show} handleClose={this.hideModal} handleConfirm={this.onConfirm}>
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

                <button className= "store-products-button-view-edit" onClick={this.showModal}>
                    <AiIcons.AiFillEdit />
                </button>
            </main>
        )
    }
}

const Modal = ({ handleClose, handleConfirm, show, children }) => {
    const showHideClassName = show ? 'modal display-block' : 'modal display-none';

    return (
        <div className={showHideClassName}>
            <section className='modal-main'>

                <div className="lines-props">
                    <h2 className="center-text">Edit Product</h2>
                    {children}
                </div>


                <div className="modal-buttons">
                    <button className="modal-buttons-props" onClick={handleClose}> Close </button>
                    <button className="modal-buttons-props" onClick={handleConfirm}> Edit </button>
                </div>


            </section>
        </div>
    );
};

EditProduct.contextType = GlobalContext;

export default EditProduct
