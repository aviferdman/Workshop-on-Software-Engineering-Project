import React from "react";
import Dialog from "@material-ui/core/Dialog";
import DialogContent from "@material-ui/core/DialogContent";
import DialogActions from "@material-ui/core/DialogActions";
import {DialogTitle, TextField} from "@material-ui/core";
import './StoreCreationForm.css'
import FormFieldInfo from "../../formsUtil/formFieldInfo";
import {GlobalContext} from "../../globalContext";
import SimpleAlertDialog from "../../components/simpleAlertDialog";
import * as HiIcons from "react-icons/hi";
import Navbar from "../../components/Navbar/Navbar";
import AddressFields from "../../formsUtil/addressFields";
import CreditCardFields from "../../formsUtil/creditCardFields";
import FormFieldCustomValidation from "../../formsUtil/formFieldCustomValidation";
import axios from "axios";
import {Link} from "react-router-dom";

export default class StoreCreateForm extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            dialogErrorMessage: '',
            showErrorDialog: false,
            storeName: new FormFieldInfo(''),
            address: new AddressFields(),
            creditCard: new CreditCardFields(),
        };
    }

    onSubmit = async e => {
        e.preventDefault();
        if (!this.validateFields()) {
            this.setState({
                ...this.state
            });
            return;
        }

        try {
            await axios.post('/Stores/Create', {
                username: this.context.username,
                storeName: this.state.storeName.value,
                address: this.state.address.valuesObject(),
                creditCard: this.state.creditCard.valuesObject({
                    number: "cardNumber"
                }),
            });
            this.props.history.push('/myStores');
        }
        catch (e) {
            let msg = (e.response && e.response.data) || e.message;
            if (msg) {
                msg = ': ' + msg;
            }
            alert('An error occurred' + msg);
            console.error("search error occurred: ", e);
        }
    };

    onCancelClick = e => {
        this.props.history.goBack();
    }

    handleChange = prop => e => {
        let newState = {
            ...this.state
        };
        newState[prop].value = e.target.value;
        this.setState(newState);
        this.state[prop].validate();
    };

    handleAddressChange = prop => e => {
        this.setState({
            address: this.state.address.setValue(prop, e.target.value)
        });
        this.state.address.getField(prop).validate();
    };

    handleCreditCardChange = prop => e => {
        this.setState({
            creditCard: this.state.creditCard.setValue(prop, e.target.value)
        });
        this.state.creditCard.getField(prop).validate();
    };

    validateFields() {
        let err = false;
        err = err || this.state.storeName.validate_required();
        err = err || this.state.address.validate();
        err = err || this.state.creditCard.validate();
        return err;
    }

    closeErrorDialog = () => {
        this.setState({
            showErrorDialog: false,
            dialogErrorMessage: ''
        });
    };

    render() {
        return (
            <div className="grid-container">
                <SimpleAlertDialog isShown={this.state.showErrorDialog} message={this.state.dialogErrorMessage} onClose={this.closeErrorDialog} />

                <header className="header-container" >
                    <a href="/">E - commerce Application</a>
                    <div>
                        <h3>{this.context.isLoggedIn ? this.context.username : ''}</h3>
                    </div>


                    <Link
                        className="icons"
                        to={{
                            pathname: "/ShoppingCart"
                        }}
                    >
                        <HiIcons.HiShoppingCart />
                    </Link>


                    <Navbar></Navbar>

                </header>

                <main>
                    <div className='center-screen store-creation-width-div'>
                        <form className='flex-form form-padding' noValidate autoComplete="off" onSubmit={this.onSubmit}>
                            <h3 className='form-title'>Store creation</h3>
                            <div className='store-creation-form-row'>
                                <TextField value={this.state.storeName.value} onChange={this.handleChange('storeName')}
                                           error={this.state.storeName.isError} helperText={this.state.storeName.errorMessage}
                                           label="Store name" variant="outlined" className='store-creation-dialog-margin' />
                            </div>
                            <div className='store-creation-complex-fields-grid-container'>
                                <div className='form-padding store-creation-form-row gray-border store-creation-margin-right'>
                                    <h4 className='store-creation-sub-form-title'>Address</h4>
                                    <div>
                                        <TextField label={'State'} variant={'outlined'} className='store-creation-margin-right'
                                                   required error={this.state.address.isError('state')} helperText={this.state.address.getErrorMessageOf('state')}
                                                   onChange={this.handleAddressChange("state")}/>
                                        <TextField label={'City'} variant={'outlined'} className='store-creation-margin-right'
                                                   required error={this.state.address.isError('city')} helperText={this.state.address.getErrorMessageOf('city')}
                                                   onChange={this.handleAddressChange("city")}/>
                                    </div>
                                    <div className='store-creation-form-row'>
                                        <TextField label={'zipCode'} variant={'outlined'}
                                                   required error={this.state.address.isError('zipCode')} helperText={this.state.address.getErrorMessageOf('zipCode')}
                                                   onChange={this.handleAddressChange("zipCode")}/>
                                    </div>
                                    <div className='store-creation-form-row'>
                                        <TextField label={'Street'} variant={'outlined'} className='store-creation-margin-right'
                                                   required error={this.state.address.isError('street')} helperText={this.state.address.getErrorMessageOf('street')}
                                                   onChange={this.handleAddressChange("street")}/>
                                        <TextField label={'Apartment number'} variant={'outlined'}
                                                   required error={this.state.address.isError('apartmentNumber')} helperText={this.state.address.getErrorMessageOf('apartmentNumber')}
                                                   onChange={this.handleAddressChange("apartmentNumber")}/>
                                    </div>
                                </div>
                                <div className='form-padding store-creation-form-row gray-border'>
                                    <h4 className='store-creation-sub-form-title'>Credit Card</h4>
                                    <div>
                                        <TextField label={'Card number'} variant={'outlined'}
                                                   required error={this.state.creditCard.isError('number')} helperText={this.state.creditCard.getErrorMessageOf('number')}
                                                   onChange={this.handleCreditCardChange("number")}/>
                                    </div>
                                    <div className='store-creation-form-row'>
                                        <TextField label={'Year'} variant={'outlined'} className='store-creation-margin-right credit-card-small-field'
                                                   required error={this.state.creditCard.isError('year')} helperText={this.state.creditCard.getErrorMessageOf('year')}
                                                   onChange={this.handleCreditCardChange("year")}/>
                                        <TextField label={'Month'} variant={'outlined'} className='store-creation-margin-right credit-card-small-field'
                                                   required error={this.state.creditCard.isError('month')} helperText={this.state.creditCard.getErrorMessageOf('month')}
                                                   onChange={this.handleCreditCardChange("month")}/>
                                        <TextField label={'CVV'} variant={'outlined'} className='credit-card-small-field'
                                                   required error={this.state.creditCard.isError('cvv')} helperText={this.state.creditCard.getErrorMessageOf('cvv')}
                                                   onChange={this.handleCreditCardChange("cvv")}/>
                                    </div>
                                    <div className='store-creation-form-row'>
                                        <TextField label={'Card holder name'} variant={'outlined'} className='store-creation-margin-right'
                                                   required error={this.state.creditCard.isError('holderName')} helperText={this.state.creditCard.getErrorMessageOf('holderName')}
                                                   onChange={this.handleCreditCardChange("holderName")}/>
                                        <TextField label={'Card holder ID'} variant={'outlined'}
                                                   required error={this.state.creditCard.isError('holderId')} helperText={this.state.creditCard.getErrorMessageOf('holderId')}
                                                   onChange={this.handleCreditCardChange("holderId")}/>
                                    </div>
                                </div>
                            </div>
                            <div className='store-creation-btn-block'>
                                <button onClick={this.onCancelClick} className='button primary store-creation-dialog-close-button'>Cancel</button>
                                <button onClick={this.onSubmit} className='button primary'>Submit</button>
                            </div>
                        </form>
                    </div>
                </main>
            </div>
        );
    }
}

StoreCreateForm.contextType = GlobalContext;
