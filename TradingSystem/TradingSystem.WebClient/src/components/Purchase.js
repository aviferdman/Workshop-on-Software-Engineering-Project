import React from "react";
import './Purchase.css';
import FormFieldInfo from "../formsUtil/formFieldInfo";
import AddressFields from "../formsUtil/addressFields";
import CreditCardFields from "../formsUtil/creditCardFields";
import axios from "axios";
import {alertRequestError_default} from "../utils";
import {GlobalContext} from "../globalContext";

class Purchase extends React.Component {
    constructor(props) {
        super(props);
        this.state = this.initState();
    }

    initState = () => {
        return {
            show: false,
            fields: {
                phoneNumber: new FormFieldInfo(),
                address: new AddressFields(),
                creditCard: new CreditCardFields(),
            }
        }
    }

    showModal = () => {
        this.setState({
            show: true
        });
    }

    hideModal = () => {
        this.setState({
            show: false
        });
    }

    onInputCompoundChange = (field, innerField) => e => {
        e.preventDefault();
        if (!this.state.fields[field].getField(innerField).trySetValueFromEvent(e)) {
            return;
        }
        this.setState({
            ...this.state
        });
    }

    onInputPrimitiveChange = field => e => {
        e.preventDefault();
        if (!this.state.fields[field].trySetValueFromEvent(e)) {
            return;
        }
        this.setState({
            ...this.state
        });
    }

    onConfirm = e => {
        e.preventDefault();
        if (!this.state.fields.phoneNumber.validate()) {
            alert('Please fill all fields');
            return;
        }
        if (!this.state.fields.creditCard.validate() ||
            !this.state.fields.address.validate()) {
            alert('Not all fields are filled or not in the correct format');
            return;
        }

        axios.post('/ShoppingCart/Purchase', {
            username: this.context.username,
            phoneNumber: this.state.fields.phoneNumber.value,
            address: this.state.fields.address.valuesObject(),
            creditCard: this.state.fields.creditCard.valuesObject({
                number: "cardNumber"
            }),
        }).then(response => {
            this.setState(this.initState());
        }, alertRequestError_default);
    }

    render() {
        return (
            <main className="items">
                <Modal show={this.state.show} handleClose={this.hideModal} handleConfirm={this.onConfirm} >
                    <div className="check-line-grid">
                        <div className= "col-grd">
                            <div className="text-props">
                                <text >Card Num</text>
                            </div>

                            <div >
                                <input
                                    type="number"
                                    placeholder="Card"
                                    className="input-props"
                                    required
                                    value={this.state.fields.creditCard.getValue('number')}
                                    onChange={this.onInputCompoundChange('creditCard', 'number')}
                                />
                            </div>
                        </div>
                        <div className= "col-grd">
                            <div className="text-props">
                                <text >Year</text>
                            </div>

                            <div >
                                <input
                                    type="number"
                                    placeholder="Year"
                                    className="input-props"
                                    required
                                    value={this.state.fields.creditCard.getValue('year')}
                                    onChange={this.onInputCompoundChange('creditCard', 'year')}
                                />
                            </div>
                        </div>

                        <div className= "col-grd">
                            <div className="text-props">
                                <text >Month</text>
                            </div>

                            <div >
                                <input
                                    type="number"
                                    placeholder="Month"
                                    className="input-props"
                                    required
                                    value={this.state.fields.creditCard.getValue('month')}
                                    onChange={this.onInputCompoundChange('creditCard', 'month')}
                                />
                            </div>
                        </div>
                        <div className= "col-grd">
                            <div className="text-props">
                                <text >Card Owner</text>
                            </div>

                            <div >
                                <input
                                    type="text"
                                    placeholder="owner"
                                    className="input-props"
                                    required
                                    value={this.state.fields.creditCard.getValue('holderName')}
                                    onChange={this.onInputCompoundChange('creditCard', 'holderName')}
                                />
                            </div>
                        </div>

                        <div className= "col-grd">
                            <div className="text-props">
                                <text >ID</text>
                            </div>

                            <div >
                                <input
                                    type="text"
                                    placeholder="id"
                                    className="input-props"
                                    required
                                    value={this.state.fields.creditCard.getValue('holderId')}
                                    onChange={this.onInputCompoundChange('creditCard', 'holderId')}
                                />
                            </div>
                        </div>

                        <div className= "col-grd">
                            <div className="text-props">
                                <text>CVV</text>
                            </div>

                            <div >
                                <input
                                    type="text"
                                    placeholder="Name"
                                    className="input-props"
                                    required
                                    value={this.state.fields.creditCard.getValue('cvv')}
                                    onChange={this.onInputCompoundChange('creditCard', 'cvv')}
                                />
                            </div>
                        </div>

                        <div className= "col-grd">
                            <div className="text-props">
                                <text >phone</text>
                            </div>

                            <div >
                                <input
                                    type="number"
                                    placeholder="phone"
                                    className="input-props"
                                    required
                                    value={this.state.fields.phoneNumber.value}
                                    onChange={this.onInputPrimitiveChange('phoneNumber')}
                                />
                            </div>
                        </div>

                        <div className= "col-grd">
                            <div className="text-props">
                                <text >state</text>
                            </div>

                            <div >
                                <input
                                    type="text"
                                    placeholder="state"
                                    className="input-props"
                                    required
                                    value={this.state.fields.address.getValue('state')}
                                    onChange={this.onInputCompoundChange('address', 'state')}
                                />
                            </div>
                        </div>

                        <div className= "col-grd">
                            <div className="text-props">
                                <text >City</text>
                            </div>

                            <div >
                                <input
                                    type="text"
                                    placeholder="City"
                                    className="input-props"
                                    required
                                    value={this.state.fields.address.getValue('city')}
                                    onChange={this.onInputCompoundChange('address', 'city')}
                                />
                            </div>
                        </div>

                        <div className= "col-grd">
                            <div className="text-props">
                                <text >Street</text>
                            </div>

                            <div>
                                <input
                                    type="text"
                                    placeholder="Street"
                                    className="input-props"
                                    required
                                    value={this.state.fields.address.getValue('street')}
                                    onChange={this.onInputCompoundChange('address', 'street')}
                                />
                            </div>
                        </div>

                        <div className= "col-grd">
                            <div className="text-props">
                                <text >Apartment Num</text>
                            </div>

                            <div >
                                <input
                                    type="number"
                                    placeholder="AP Num"
                                    className="input-props"
                                    required
                                    value={this.state.fields.address.getValue('apartmentNumber')}
                                    onChange={this.onInputCompoundChange('address', 'apartmentNumber')}

                                />
                            </div>
                        </div>

                        <div className= "col-grd">
                            <div className="text-props">
                                <text >Zip</text>
                            </div>

                            <div >
                                <input
                                    type="number"
                                    placeholder="Zip"
                                    className="input-props"
                                    required
                                    value={this.state.fields.address.getValue('zipCode')}
                                    onChange={this.onInputCompoundChange('address', 'zipCode')}
                                />
                            </div>
                        </div>


                    </div>


                </Modal>

                <button className= "store-products-button-view" onClick={this.showModal}> Purchase </button>
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
                    <h2 className="center-text">Purchase</h2>
                    {children}
                </div>


                <div className="modal-buttons">
                    <button className="modal-buttons-props" onClick={handleClose} > Close </button>
                    <button className="modal-buttons-props" onClick={handleConfirm} > Purchase </button>
                </div>


            </section>
        </div>
    );
};

Purchase.contextType = GlobalContext;

export default Purchase
