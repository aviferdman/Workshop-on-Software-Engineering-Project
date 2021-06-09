import React from "react";
import './Policy.css';
import { GlobalContext } from "../../../globalContext";
import FormFields from "../../../formsUtil/formFields";
import NumberFormField from "../../../formsUtil/NumberFormField";
import NullFormField from "../../../formsUtil/NullFormField";
import NullableNumberFormField from "../../../formsUtil/NullableNumberFormField";
import DateFormField from "../../../formsUtil/DateFormField";
import * as api from "../../../api";
import {alertRequestError_default} from "../../../utils";
import ConditionalRender from "../../../ConditionalRender";


class AddPolicy extends React.Component {
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

    newFields() {
        return new FormFields({
            ruleRelation: 'Simple',
            ruleContext: 'Product',
            ruleType: '',

            productId: new NullFormField(),
            category: '',
            minValue: new NullableNumberFormField(),
            maxValue: new NullableNumberFormField(),
            startDate: new DateFormField(),
            endDate: new DateFormField(),
        });
    }

    resetState = set => {
        let state = {
            show: false,
            fields: this.newFields(),
            minMaxStep: 1,
        };
        if (!set) {
            this.state = state;
        }
        else {
            this.setState(state);
        }

        this.state.fields.getField('ruleType').setValidationOff();
        this.state.fields.fields.minValue.setValidationOff();
        this.state.fields.fields.maxValue.setValidationOff();
        this.state.fields.fields.startDate.setValidationOff();
        this.state.fields.fields.endDate.setValidationOff();

        this.onRuleContextChangeCore();
        this.onRuleTypeChangeCore();
        if (set) {
            this.setState({
                ...this.state
            });
        }
    }

    getField = field => {
        return this.state.fields.getField(field);
    }

    getFieldValue = field => {
        return this.state.fields.getValue(field);
    }

    getInputValue = field => {
        return this.getField(field).inputValue;
    }

    onInputChange = field => e => {
        if (!this.state.fields.getField(field).trySetValueFromEvent(e)) {
            return;
        }
        this.setState({
            ...this.state
        });
    }

    onRuleTypeChange = e => {
        if (!this.state.fields.getField('ruleType').trySetValueFromEvent(e)) {
            return;
        }

        this.onRuleTypeChangeCore();
        this.setState({
            ...this.state
        });
    }

    onRuleTypeChangeCore = () => {
        let ruleType = this.getFieldValue('ruleType');
        switch (ruleType) {
            case '':
                break;

            case 'Time':
                break;

            default:
                switch (ruleType) {
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
    }

    onRuleContextChange = e => {
        if (!this.state.fields.getField('ruleContext').trySetValueFromEvent(e)) {
            return;
        }

        this.onRuleContextChangeCore();
        this.setState({
            ...this.state
        });
    }

    onRuleContextChangeCore = () => {
        let ruleContext = this.getFieldValue('ruleContext');
        switch (ruleContext) {
            case 'Product':
                this.state.fields.fields.productId.setValidationOn();
                this.state.fields.fields.category.setValidationOff();
                break;

            case 'Category':
                this.state.fields.fields.productId.setValidationOff();
                this.state.fields.fields.category.setValidationOn();
                break;

            case 'Store':
                this.state.fields.fields.productId.setValidationOff();
                this.state.fields.fields.category.setValidationOff();
                break;

            default:
                throw new Error('Invalid discount type');
        }
    }

    onConfirm = async e => {
        e.preventDefault();
        if (!this.state.fields.validate()) {
            alert('Please fill all required fields');
            return;
        }

        let policy = this.state.fields.valuesObject();

        switch (policy.ruleContext) {
            case 'Product':
                policy.category = "";
                break;
            case 'Category':
                policy.productId = null;
                break;
            case 'Store':
                policy.productId = null;
                policy.category = "";
                break;
            default:
                throw new Error('Invalid discount type');
        }

        switch (policy.ruleType) {
            case '':
                policy.minValue = null;
                policy.maxValue = null;
                policy.startDate = null;
                policy.endDate = null;
                break;

            case 'Time':
                if (policy.startDate == null && policy.endDate == null) {
                    alert('Please fill all required fields');
                    return;
                }
                policy.minValue = null;
                policy.maxValue = null;
                break;

            default:
                if (policy.minValue == null && policy.maxValue == null) {
                    alert('Please fill all required fields');
                    return;
                }
                policy.startDate = null;
                policy.endDate = null;
                break;
        }

        let reqData = Object.assign({}, policy, {
            username: this.context.username,
            storeId: this.props.storeId,
        });
        await api.stores.policies.add(reqData).then(() => {
            policy.creator = this.context.username;
            this.props.onSuccess(policy);
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
                                    <label>Policy Rule</label>
                                </div>

                                <div>
                                    <select className="disc-input-props"
                                            required
                                            value={this.getInputValue('ruleRelation')}
                                            onChange={this.onInputChange('ruleRelation')}>
                                        <option value="Simple">Simple</option>
                                        <option value="Condition">Xor</option>
                                        <option value="And">And</option>
                                        <option value="Or">Or</option>
                                    </select>
                                </div>
                            </div>
                        </div>

                        <div>
                            <div className= "disc-col-grd-perm">
                                <div className="disc-text-props">
                                    <label>Policy Context</label>
                                </div>

                                <div>
                                    <select className="disc-input-props"
                                            required
                                            value={this.getInputValue('ruleContext')}
                                            onChange={this.onRuleContextChange}>
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
                                    <label>Rule Type</label>
                                </div>

                                <div>
                                    <select className="disc-input-props"
                                            required
                                            value={this.getInputValue('ruleType')}
                                            onChange={this.onRuleTypeChange}>
                                        <option value="">Simple</option>
                                        <option value="Quantity">Quantity</option>
                                        <option value="Weight">Weight</option>
                                        <option value="Price">Price</option>
                                        <option value="Time">Time</option>
                                    </select>
                                </div>
                            </div>
                        </div>

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

                        <ConditionalRender
                            condition={this.getFieldValue('ruleType') !== '' && this.getFieldValue('ruleType') !== 'Time'}
                            render={() => (
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
                                                value={this.getInputValue('minValue')}
                                                onChange={this.onInputChange('minValue')}
                                            />
                                        </div>
                                    </div>
                                </div>
                            )}
                        />

                        <ConditionalRender
                            condition={this.getFieldValue('ruleType') !== '' && this.getFieldValue('ruleType') !== 'Time'}
                            render={() => (
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
                                                value={this.getInputValue('maxValue')}
                                                onChange={this.onInputChange('maxValue')}
                                            />
                                        </div>
                                    </div>
                                </div>
                            )}
                        />

                        <ConditionalRender
                            condition={this.getFieldValue('ruleType') === 'Time'}
                            render={() => (
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
                                                value={this.getInputValue('startDate')}
                                                onChange={this.onInputChange('startDate')}
                                            />
                                        </div>
                                    </div>
                                </div>
                            )}
                        />

                        <ConditionalRender
                            condition={this.getFieldValue('ruleType') === 'Time'}
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

                    </div>

                </Modal>

                <button className= "store-products-button-view" onClick={this.showModal}> Add Policy </button>
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
                    <h2 className="center-text">Add Policy</h2>
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

AddPolicy.contextType = GlobalContext;
export default AddPolicy
