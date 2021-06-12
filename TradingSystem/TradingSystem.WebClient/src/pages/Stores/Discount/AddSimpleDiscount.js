import React from "react";
import './AddSimpleDiscount.css';
import { GlobalContext } from "../../../globalContext";
import * as api from "../../../api";
import FormFields from "../../../formsUtil/formFields";
import {alertRequestError_default} from "../../../utils";
import NumberFormField from "../../../formsUtil/NumberFormField";
import NullFormField from "../../../formsUtil/NullFormField";
import DateFormField from "../../../formsUtil/DateFormField";
import ConditionalRender from "../../../ConditionalRender";
import NullableNumberFormField from "../../../formsUtil/NullableNumberFormField";

class AddSimpleDiscount extends React.Component {
    constructor(props) {
        super(props);
        this.resetState(false);
    }

    showModal = () => {
        this.setState({ show: true });
    }

    hideModal = () => {
        this.setState({ show: false });
    }

    resetState = set => {
        let state = {
            show: false,
            discountFields: this.newFields(),
            minMaxStep: 1,
        };
        if (!set) {
            this.state = state;
        }
        else {
            this.setState(state);
        }

        this.state.discountFields.getField('conditionType').setValidationOff();
        this.state.discountFields.fields.minValue.setValidationOff();
        this.state.discountFields.fields.maxValue.setValidationOff();
        this.state.discountFields.fields.startDate.setValidationOff();
        this.state.discountFields.fields.endDate.setValidationOff();

        this.onConditionTypeChangeCore();
        if (set) {
            this.setState({
                ...this.state
            });
        }
    }

    newFields() {
        return new FormFields({
            discountType: 'Product',
            conditionType: '',
            percent: new NumberFormField(),
            productId: new NullFormField(),
            category: '',
            minValue: new NullableNumberFormField(),
            maxValue: new NullableNumberFormField(),
            startDate: new DateFormField(),
            endDate: new DateFormField(),
        });
    }

    getField = field => {
        return this.state.discountFields.getField(field);
    }

    getFieldValue = field => {
        return this.state.discountFields.getValue(field);
    }

    getInputValue = field => {
        return this.getField(field).inputValue;
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
        let conditionType = this.getFieldValue('conditionType');
        switch (conditionType) {
            case '':
                break;

            case 'Time':
                break;

            default:
                switch (conditionType) {
                    case 'Quantity':
                        this.state.minMaxStep = '1';
                        break;
                    default:
                        this.state.minMaxStep = '0.01';
                        break;
                }
                this.getField('minValue').inputValue = '';
                this.getField('minValue').value = null;
                this.getField('maxValue').inputValue = '';
                this.getField('maxValue').value = null;
                break;
        }

        this.onDiscountTypeChangeCore();
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
        let discountType = this.getFieldValue('discountType');
        let conditionType = this.getFieldValue('conditionType');

        if (conditionType !== 'Quantity') {
            this.state.discountFields.fields.productId.setValidationOff();
            this.state.discountFields.fields.category.setValidationOff();
            return;
        }

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
            alert('Please fill all required fields');
            return;
        }

        let discountObj = this.state.discountFields.valuesObject();
        discountObj.percent = discountObj.percent / 100;

        switch (discountObj.discountType) {
            case 'Product':
                discountObj.category = "";
                break;
            case 'Category':
                discountObj.productId = null;
                break;
            case 'Store':
                discountObj.productId = null;
                discountObj.category = "";
                break;
            default:
                throw new Error('Invalid discount type');
        }

        switch (discountObj.conditionType) {
            case '':
                discountObj.minValue = null;
                discountObj.maxValue = null;
                discountObj.startDate = null;
                discountObj.endDate = null;
                break;

            case 'Time':
                if (this.getFieldValue('startDate') == null && this.getFieldValue('endDate') == null) {
                    alert('Please fill all required fields');
                    return;
                }
                discountObj.minValue = null;
                discountObj.maxValue = null;
                break;

            default:
                if (this.getFieldValue('minValue') == null && this.getFieldValue('maxValue') == null) {
                    alert('Please fill all required fields');
                    return;
                }
                discountObj.startDate = null;
                discountObj.endDate = null;
                break;
        }

        let promise = api.stores.discounts.add({
            username: this.context.username,
            storeId: this.props.storeId,
            params: discountObj,
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

                                <div>
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

                        <ConditionalRender
                            condition={this.getFieldValue('conditionType') === 'Quantity'}
                            render={() => (
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
                            )}
                        />

                        <ConditionalRender
                            condition={this.getFieldValue('conditionType') !== '' && this.getFieldValue('conditionType') !== 'Time'}
                            render={() => (
                                <div className='disc-grid-col-1-1'>
                                    <div className= "disc-col-grd-perm">
                                        <div className="disc-text-props">
                                            <label>Min</label>
                                        </div>

                                        <div >
                                            <input
                                                type="number"
                                                step={this.state.minMaxStep}
                                                className="disc-input-props"
                                                value={this.getInputValue('minValue')}
                                                onChange={this.onInputChange('minValue')}
                                            />
                                        </div>
                                    </div>
                                </div>
                            )}
                        />

                        <ConditionalRender
                            condition={this.getFieldValue('conditionType') !== '' && this.getFieldValue('conditionType') !== 'Time'}
                            render={() => (
                                <div>
                                    <div className= "disc-col-grd-perm">
                                        <div className="disc-text-props">
                                            <label>Max</label>
                                        </div>

                                        <div >
                                            <input
                                                type="number"
                                                step={this.state.minMaxStep}
                                                className="disc-input-props"
                                                value={this.getInputValue('maxValue')}
                                                onChange={this.onInputChange('maxValue')}
                                            />
                                        </div>
                                    </div>
                                </div>
                            )}
                        />

                        <ConditionalRender
                            condition={this.getFieldValue('conditionType') === 'Time'}
                            render={() => (
                                <div className='disc-grid-col-1-1'>
                                    <div className= "disc-col-grd-perm">
                                        <div className="disc-text-props">
                                            <label>Start Date</label>
                                        </div>

                                        <div >
                                            <input
                                                type="date"
                                                className="disc-input-props"
                                                style={{width: "15rem" , height: "3rem" }}
                                                value={this.getInputValue('startDate')}
                                                onChange={this.onInputChange('startDate')}
                                            />
                                        </div>
                                    </div>
                                </div>
                            )}
                        />

                        <ConditionalRender
                            condition={this.getFieldValue('conditionType') === 'Time'}
                            render={() => (
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
                                                value={this.getInputValue('endDate')}
                                                onChange={this.onInputChange('endDate')}
                                            />
                                        </div>
                                    </div>
                                </div>
                            )}
                        />

                        <ConditionalRender
                            condition={this.getField('productId').isValidationOn}
                            render={() => (
                                <div>
                                    <div className= "disc-col-grd-perm">
                                        <div className="disc-text-props">
                                            <label>Product</label>
                                        </div>

                                        <select className="disc-input-props"
                                                required
                                                value={this.getInputValue('productId')}
                                                onChange={this.onInputChange('productId')}>
                                            <option value=""/>
                                            <ConditionalRender
                                                condition={this.props.storeProducts != null}
                                                render={() => this.props.storeProducts.map(product => {
                                                    return (<option value={product.id} key={product.id}>{product.name}</option>);
                                                })}
                                            />
                                        </select>
                                    </div>
                                </div>
                            )}
                        />

                        <ConditionalRender
                            condition={this.getField('category').isValidationOn}
                            render={() => (
                                <div>
                                    <div className= "disc-col-grd-perm">
                                        <div className="disc-text-props">
                                            <label>Category</label>
                                        </div>

                                        <div >
                                            <input
                                                type="text"
                                                className="disc-input-props"
                                                value={this.getInputValue('category')}
                                                onChange={this.onInputChange('category')}
                                            />
                                        </div>
                                    </div>
                                </div>
                            )}
                        />

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
