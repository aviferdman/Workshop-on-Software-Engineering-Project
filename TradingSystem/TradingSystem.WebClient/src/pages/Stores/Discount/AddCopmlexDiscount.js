import React from "react";
import './AddSimpleDiscount.css';
import { GlobalContext } from "../../../globalContext";
import FormFields from "../../../formsUtil/formFields";
import * as api from "../../../api";
import {alertRequestError_default} from "../../../utils";
import CheckboxFormField from "../../../formsUtil/CheckboxFormField";
import ConditionalRender from "../../../ConditionalRender";


class AddComplexDiscount extends React.Component {
    constructor(props) {
        super(props);
        this.resetState(false);
    }

    componentDidMount() {
        this.resetState(true);
    }

    showModal = () => {
        this.setState({ show: true });
    }

    hideModal = () => {
        this.setState({ show: false });
    }

    resetState(set) {
        let state = {
            show: false,
            discountFields: new FormFields({
                discountRuleRelation: 'Xor',
                discountId1: '',
                discountId2: '',
                decision: new CheckboxFormField(),
            }),
            showDecision: true
        };
        if (set) {
            this.setState(state);
        }
        else {
            this.state = state;
        }
    }

    getField = field => {
        return this.state.discountFields.getField(field);
    }

    getInputValue = field => {
        return this.getField(field).inputValue;
    }

    onRelationChange = e => {
        e.preventDefault();
        let relationField = this.state.discountFields.fields.discountRuleRelation;
        if (!relationField.trySetValueFromEvent(e)) {
            return;
        }

        let relation = relationField.getValue();
        this.setState({
            showDecision: relation === 'Xor',
        });
    }

    onInputChange = field => e => {
        if (!this.state.discountFields.getField(field).trySetValueFromEvent(e)) {
            return;
        }
        this.setState({
            ...this.state
        });
    }

    onConfirm = async e => {
        e.preventDefault();
        if (!this.state.discountFields.validate()) {
            alert('Please fill all required fields');
            return;
        }

        let discountObj = this.state.discountFields.valuesObject();
        let reqData = Object.assign({}, discountObj, {
            username: this.context.username,
            storeId: this.props.storeId,
        });
        await api.stores.discounts.addCompound(reqData)
            .then(discountId => {
                discountObj.id = discountId;
                discountObj.creator = this.context.username;
                this.props.onSuccess(discountObj);
                this.resetState(true);
            }, alertRequestError_default)
    }

    render() {
        return (
            <main className="items">
                <Modal show={this.state.show} handleClose={this.hideModal} handleConfirm={this.onConfirm} >

                    <div className="disc-comp-check-line-grid">

                        <div className="center-item">
                            <div className= "disc-col-grd-perm">
                                <div className="disc-text-props">
                                    <label>Discount Relation</label>
                                </div>

                                <div>
                                    <select className="disc-input-props"
                                            required
                                            value={this.getInputValue('discountRuleRelation')}
                                            onChange={this.onRelationChange}>
                                        <option value="Xor">Xor</option>
                                        <option value="Or">Or</option>
                                        <option value="And">And</option>
                                    </select>
                                </div>
                            </div>
                        </div>

                        <div className="center-item">
                            <div className= "disc-col-grd-perm">
                                <div className="disc-text-props">
                                    <label>First Discount</label>
                                </div>

                                <div >
                                    <input
                                        type="text"
                                        className="disc-input-props"
                                        required
                                        value={this.getInputValue('discountId1')}
                                        onChange={this.onInputChange('discountId1')}
                                    />
                                </div>
                            </div>
                        </div>

                        <div className="center-item">
                            <div className= "disc-col-grd-perm">
                                <div className="disc-text-props">
                                    <label>Second Discount</label>
                                </div>

                                <div >
                                    <input
                                        type="text"
                                        className="disc-input-props"
                                        required
                                        value={this.getInputValue('discountId2')}
                                        onChange={this.onInputChange('discountId2')}
                                    />
                                </div>
                            </div>
                        </div>

                        <ConditionalRender
                            condition={this.getField('discountRuleRelation').getValue() === 'Xor'}
                            render={() => (
                                <div className="center-item">
                                    <div className= "disc-col-grd-perm">
                                        <div className="disc-text-props">
                                            <label>Decision</label>
                                        </div>

                                        <div >
                                            <input
                                                type="checkbox"
                                                className="disc-input-props"
                                                value={'decision'}
                                                checked={this.getInputValue('decision')}
                                                onChange={this.onInputChange('decision')}
                                            />
                                        </div>
                                    </div>
                                </div>
                            )}
                        />

                    </div>

                </Modal>

                <button className= "store-products-button-view" onClick={this.showModal}> Add Complex Discount </button>
            </main>
        )
    }
}

const Modal = ({ handleClose, handleConfirm, show, children }) => {
    const showHideClassName = show ? 'modal display-block' : 'modal display-none';

    return (
        <div className={showHideClassName}>
            <section className='disc-comp-modal-main'>

                <div className="lines-props">
                    <h2 className="comp-center-text">Add Complex Discount</h2>
                    {children}
                </div>


                <div className="comp-modal-buttons">
                    <button className="modal-buttons-props" onClick={handleClose} > Close </button>
                    <button className="modal-buttons-props" onClick={handleConfirm} > Add </button>
                </div>


            </section>
        </div>
    );
};

AddComplexDiscount.contextType = GlobalContext;
export default AddComplexDiscount
