import React from "react";
import './AddSimpleDiscount.css';
import { GlobalContext } from "../../../globalContext";
import * as api from "../../../api";
import FormFields from "../../../formsUtil/formFields";
import {alertRequestError_default} from "../../../utils";
import NumberFormField from "../../../formsUtil/NumberFormField";

class AddSimpleDiscount extends React.Component {
    constructor(props) {
        super(props);
        this.resetState(false);
    }

    resetState = set => {
        let state = {
            show: false,
            discountFields: this.newFields(),
        };
        if (!set) {
            this.state = state;
        }
        this.state.discountFields.getField('conditionType').setValidationOff();
        this.onDiscountTypeChangeCore();
        if (set) {
            this.setState(state);
        }
    }

    newFields() {
        return new FormFields({
            discountType: 'Product',
            conditionType: '',
            percent: new NumberFormField(''),
            productId: '',
            category: '',
        });
    }

    showModal = () => {
        this.setState({ show: true });
    }

    hideModal = () => {
        this.setState({ show: false });
    }

    getInputValue = field => {
        return this.state.discountFields.getValue(field);
    }

    onInputChange = field => e => {
        e.preventDefault();
        if (!this.state.discountFields.getField(field).trySetValueFromEvent(e)) {
            return;
        }
        this.setState({
            ...this.state
        });
    }

    onConditionTypeChange = e => {
        e.preventDefault();
        if (!this.state.discountFields.getField('conditionType').trySetValueFromEvent(e)) {
            return;
        }

        this.onConditionTypeChangeCore();
        this.setState({
            ...this.state
        });
    }

    onConditionTypeChangeCore = () => {
        let conditionType = this.getInputValue('conditionType');
        switch (conditionType) {
            case '':
                break;

            case 'Time':
                break;

            default:
                break;
        }
    }

    onDiscountTypeChange = e => {
        e.preventDefault();
        if (!this.state.discountFields.getField('discountType').trySetValueFromEvent(e)) {
            return;
        }

        this.onDiscountTypeChangeCore();
        this.setState({
            ...this.state
        });
    }

    onDiscountTypeChangeCore = () => {
        let discountType = this.getInputValue('discountType');
        switch (discountType) {
            case 'Product':
                this.state.discountFields.fields.productId.setValidationOn();
                this.state.discountFields.fields.category.setValidationOff();
                break;

            case 'Category':
                this.state.discountFields.fields.productId.setValidationOff();
                this.state.discountFields.fields.category.setValidationOn();
                break;

            case 'Store':
                this.state.discountFields.fields.productId.setValidationOff();
                this.state.discountFields.fields.category.setValidationOff();
                break;

            default:
                throw new Error('Invalid discount type');
        }
    }

    onConfirm = async e => {
        e.preventDefault();
        if (!this.state.discountFields.validate()) {
            alert('Please fill all fields');
            return;
        }

        let discountObj = this.state.discountFields.valuesObject();
        let promise = null;


        if (this.getInputValue('conditionType')) {
            // TODO: remove later
            alert('TODO: condition discount');
            return;
        }


        discountObj.percent = discountObj.percent / 100;
         promise = api.stores.discounts.addSimple({
             username: this.context.username,
             storeId: this.props.storeId,
             discountType: discountObj.discountType,
             percent: discountObj.percent,
             category: discountObj.category,
             productId: discountObj.productId ? discountObj.productId : null,
         });
         await promise.then(discountId => {
             discountObj.id = discountId;
             discountObj.creator = this.context.username;
             this.props.onSuccess(discountObj);
             this.resetState(true);
         }, alertRequestError_default);
    }

    render() {
        return (
            <main className="items">
                <Modal show={this.state.show} handleClose={this.hideModal} handleConfirm={this.onConfirm}  >

                    <div className="disc-check-line-grid">

                        <div>
                            <div className= "disc-col-grd-perm">
                                <div className="disc-text-props">
                                    <label>Discount Type</label>
                                </div>

                                <div>
                                    <select className="disc-input-props"
                                            required
                                            value={this.getInputValue('discountType')}
                                            onChange={this.onDiscountTypeChange}>
                                        <option value="Product">Product</option>
                                        <option value="Category">Category</option>
                                        <option value="Store">Store</option>
                                    </select>
                                </div>
                            </div>
                        </div>

                        <div>
                            <div className= "disc-col-grd-perm">
                                <div className="disc-text-props">
                                    <label>Condition</label>
                                </div>

                                <div>
                                    <select className="disc-input-props"
                                            required
                                            value={this.getInputValue('conditionType')}
                                            onChange={this.onConditionTypeChange}>
                                        <option value="">None</option>
                                        <option value="Quantity">Quantity</option>
                                        <option value="Weight">Weight</option>
                                        <option value="Price">Price</option>
                                        <option value="Time">Time</option>
                                    </select>
                                </div>
                            </div>
                        </div>

                        <div>
                            <div className= "disc-col-grd-perm">
                                <div className="disc-text-props">
                                    <label>Percent</label>
                                </div>

                                <div >
                                    <input
                                        type="number"
                                        step="0.01"
                                        className="disc-input-props"
                                        required
                                        value={this.getInputValue('percent')}
                                        onChange={this.onInputChange('percent')}
                                    />
                                </div>
                            </div>
                        </div>

                        <div>
                            <div className= "disc-col-grd-perm">
                                <div className="disc-text-props">
                                    <label>Product</label>
                                </div>

                                <div >
                                    <input
                                        type="text"
                                        className="disc-input-props"
                                        required
                                        value={this.getInputValue('productId')}
                                        onChange={this.onInputChange('productId')}
                                    />
                                </div>
                            </div>
                        </div>

                        <div>
                            <div className= "disc-col-grd-perm">
                                <div className="disc-text-props">
                                    <label>Category</label>
                                </div>

                                <div >
                                    <input
                                        type="text"
                                        className="disc-input-props"
                                        required
                                        value={this.getInputValue('category')}
                                        onChange={this.onInputChange('category')}
                                    />
                                </div>
                            </div>
                        </div>

                        <div>
                            <div className= "disc-col-grd-perm">
                                <div className="disc-text-props">
                                    <label>Min</label>
                                </div>

                                <div >
                                    <input
                                        type="number"
                                        step="0.01"
                                        className="disc-input-props"
                                    />
                                </div>
                            </div>
                        </div>

                        <div>
                            <div className= "disc-col-grd-perm">
                                <div className="disc-text-props">
                                    <label>Max</label>
                                </div>

                                <div >
                                    <input
                                        type="number"
                                        step="0.01"
                                        className="disc-input-props"
                                    />
                                </div>
                            </div>
                        </div>

                        <div>
                            <div className= "disc-col-grd-perm">
                                <div className="disc-text-props">
                                    <label>Start Date</label>
                                </div>

                                <div >
                                    <input
                                        type="date"
                                        className="disc-input-props"
                                        style={{width: "15rem" , height: "3rem" }}
                                    />
                                </div>
                            </div>
                        </div>

                        <div>
                            <div className= "disc-col-grd-perm">
                                <div className="disc-text-props">
                                    <label>End Date</label>
                                </div>

                                <div >
                                    <input
                                        type="date"
                                        className="disc-input-props"
                                        style={{width: "15rem" , height: "3rem" }}

                                    />
                                </div>
                            </div>
                        </div>

                    </div>

                </Modal>

                <button className= "store-products-button-view" onClick={this.showModal}> Add Simple Discount </button>
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
                    <h2 className="center-text">Add Simple Discount</h2>
                    {children}
                </div>


                <div className="modal-buttons">
                    <button className="modal-buttons-props" onClick={handleClose} > Close </button>
                    <button className="modal-buttons-props" onClick={handleConfirm} > Add </button>
                </div>


            </section>
        </div>
    );
};

AddSimpleDiscount.contextType = GlobalContext;
export default AddSimpleDiscount
