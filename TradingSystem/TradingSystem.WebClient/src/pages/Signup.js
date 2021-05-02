import {guestUsername, useTitle} from "../App";
import {useHistory} from "react-router-dom";
import {Button, makeStyles, TextField} from "@material-ui/core";
import PasswordField from "../components/passwordFields";
import React from "react";
import clsx from "clsx";
import FormFieldInfo from "../formFieldInfo";
import axios from "axios";
import useFormsStyles from "../style/forms";
import SimpleAlertDialog from "../components/simpleAlertDialog";

export default function Signup() {
    const history = useHistory();
    useTitle('Sign up');

    const [state, setState] = React.useState({
        username: new FormFieldInfo(''),
        password: new FormFieldInfo(''),
        confirmPassword: new FormFieldInfo(''),
        phoneNumber: new FormFieldInfo(''),
        state: new FormFieldInfo(''),
        city: new FormFieldInfo(''),
        street: new FormFieldInfo(''),
        apartmentNumber: new FormFieldInfo(''),
        dialogErrorMessage: '',
        showErrorDialog: false
    });

    const useStyles = makeStyles((theme) => ({
        formRow: {
            marginTop: theme.spacing(1),
            alignSelf: 'flex-start'
        },
        formInRowInput: {
            marginRight: theme.spacing(1)
        },
        btnBlock: {
            alignSelf: 'flex-end'
        },
        btnCancel: {
            marginRight: theme.spacing(4),
        },
    }));
    const classes = useStyles();
    const formsClasses = useFormsStyles();

    const closeErrorDialog = () => {
        setState({
            ...state,
            showErrorDialog: false,
            dialogErrorMessage: ''
        });
    };

    function onCancelClick(e) {
        history.goBack();
    }

    const handleChange = prop => e => {
        let newState = {
            ...state
        };
        newState[prop].value = e.target.value;
        setState(newState);
    };

    async function onSubmit(e) {
        e.preventDefault();
        let formError = false;
        let newState = {
            ...state
        };

        if (!state.username.value) {
            formError = true;
            newState.username.isError = true;
            newState.username.errorMessage = 'Required';
        }
        else {
            newState.username.isError = false;
            newState.username.errorMessage = '';
        }
        if (!state.password.value) {
            formError = true;
            newState.password.isError = true;
            newState.password.errorMessage = 'Required';
        }
        else {
            newState.password.isError = false;
            newState.password.errorMessage = '';
        }
        if (!state.confirmPassword.value) {
            formError = true;
            newState.confirmPassword.isError = true;
            newState.confirmPassword.errorMessage = 'Required';
        }
        if (state.password.value &&
            state.confirmPassword.value) {
            if (state.password.value !== state.confirmPassword.value) {
                formError = true;
                newState.confirmPassword.isError = true;
                newState.confirmPassword.errorMessage = 'Passwords do not match';
            }
            else {
                newState.confirmPassword.isError = false;
                newState.confirmPassword.errorMessage = '';
            }
        }
        else if (state.confirmPassword.value && !state.password.value) {
            newState.confirmPassword.isError = false;
            newState.confirmPassword.errorMessage = '';
        }
        if (!state.phoneNumber.value) {
            formError = true;
            newState.phoneNumber.isError = true;
            newState.phoneNumber.errorMessage = 'Required';
        }
        else {
            let match = /(\d{3})(-?)(\d{7})/.exec(state.phoneNumber.value);
            if (match.length === 0) {
                newState.phoneNumber.isError = true;
                newState.phoneNumber.errorMessage = 'Invalid format';
            }
            else {
                if (match[2]) {
                    newState.phoneNumber.value = match[1] + match[3];
                }

                newState.phoneNumber.isError = false;
                newState.phoneNumber.errorMessage = '';
            }
        }
        if (!state.state.value) {
            formError = true;
            newState.state.isError = true;
            newState.state.errorMessage = 'Required';
        }
        else {
            newState.state.isError = false;
            newState.state.errorMessage = '';
        }
        if (!state.city.value) {
            formError = true;
            newState.city.isError = true;
            newState.city.errorMessage = 'Required';
        }
        else {
            newState.city.isError = false;
            newState.city.errorMessage = '';
        }
        if (!state.street.value) {
            formError = true;
            newState.street.isError = true;
            newState.street.errorMessage = 'Required';
        }
        else {
            newState.street.isError = false;
            newState.street.errorMessage = '';
        }
        if (!state.apartmentNumber.value) {
            formError = true;
            newState.apartmentNumber.isError = true;
            newState.apartmentNumber.errorMessage = 'Required';
        }
        else {
            if (!/\d+/.test(state.apartmentNumber.value)) {
                formError = true;
                newState.apartmentNumber.isError = true;
                newState.apartmentNumber.errorMessage = 'Invalid format';
            }
            else {
                newState.apartmentNumber.isError = false;
                newState.apartmentNumber.errorMessage = '';
            }
        }

        setState(newState);
        if (!formError) {
            try {
                await axios.post('/UserGateway/SignUp', {
                    guestusername: guestUsername,
                    username: state.username.value,
                    password: state.password.value,
                    _state: state.state.value,
                    _city: state.city.value,
                    _street: state.street.value,
                    _apartmentNum: state.apartmentNumber.value,
                    phone: state.phoneNumber.value
                });
                history.push('/login');
            }
            catch (e) {
                let errMsg = '';
                if (e.response.status === 400) {
                    if (e.response.data.startsWith('username: ' + state.username.value + ' is already taken')) {
                        errMsg = 'Username \'' + state.username.value + '\' is already taken. Please choose a different one.'
                    }
                }
                if (!errMsg) {
                    errMsg = 'Unknown error occurred: ' + e.message;
                }
                setState({
                    ...state,
                    dialogErrorMessage: errMsg,
                    showErrorDialog: true
                });
            }
        }
    }

    return (
        <div>
            <SimpleAlertDialog isShown={state.showErrorDialog} message={state.dialogErrorMessage} onClose={closeErrorDialog} />
            <div className={formsClasses.center}>
                <form className={formsClasses.form} onSubmit={onSubmit} noValidate autoComplete="off">
                    <h4 className={formsClasses.title}>Sign up</h4>
                    <TextField label={'Username'} variant={'outlined'} className={classes.formRow}
                               required error={state.username.isError} helperText={state.username.errorMessage}
                               onChange={handleChange("username")}/>
                    <div className={classes.formRow}>
                        <PasswordField label={'Password'} variant={'outlined'} className={classes.formInRowInput}
                                       required error={state.password.isError} helperText={state.password.errorMessage}
                                       onChange={handleChange("password")} id={'input-password'}/>
                        <PasswordField label={'Confirm password'} variant={'outlined'}
                                       required error={state.confirmPassword.isError} helperText={state.confirmPassword.errorMessage}
                                       onChange={handleChange("confirmPassword")} id={'input-password-confirm'}/>
                    </div>
                    <TextField label={'Phone number'} variant={'outlined'} className={classes.formRow}
                               required error={state.phoneNumber.isError} helperText={state.phoneNumber.errorMessage}
                               onChange={handleChange("phoneNumber")}/>
                    <div className={classes.formRow}>
                        <TextField label={'State'} variant={'outlined'} className={classes.formInRowInput}
                                   required error={state.state.isError} helperText={state.state.errorMessage}
                                   onChange={handleChange("state")}/>
                        <TextField label={'City'} variant={'outlined'}
                                   required error={state.city.isError} helperText={state.city.errorMessage}
                                   onChange={handleChange("city")}/>
                    </div>
                    <div className={classes.formRow}>
                        <TextField label={'Street'} variant={'outlined'} className={classes.formInRowInput}
                                   required error={state.street.isError} helperText={state.street.errorMessage}
                                   onChange={handleChange("street")}/>
                        <TextField label={'Apartment number'} variant={'outlined'}
                                   required error={state.apartmentNumber.isError} helperText={state.apartmentNumber.errorMessage}
                                   onChange={handleChange("apartmentNumber")}/>
                    </div>
                    <div className={clsx(classes.btnBlock, classes.formRow)}>
                        <Button variant="contained" color="primary" className={classes.btnCancel} onClick={onCancelClick}>Cancel</Button>
                        <Button type={"submit"} variant="contained" color="primary">Confirm</Button>
                    </div>
                </form>
            </div>
        </div>
    );
}