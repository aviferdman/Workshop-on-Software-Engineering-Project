import React from "react";

import {useTitle} from "../../App";
import {useHistory} from "react-router-dom";
import {Button, makeStyles, TextField} from "@material-ui/core";
import PasswordField from "../../components/passwordFields";
import clsx from "clsx";
import FormFieldInfo from "../../formsUtil/formFieldInfo";
import axios from "axios";
import useFormsStyles from "../../style/forms";
import SimpleAlertDialog from "../../components/simpleAlertDialog";
import {GlobalContext} from "../../globalContext";

export default function Signup() {
    const history = useHistory();
    useTitle('Sign up');

    const [state, setState] = React.useState({
        username: new FormFieldInfo(''),
        password: new FormFieldInfo(''),
        phoneNumber: new FormFieldInfo(''),
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
            textAlign: 'right',
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

    const makeOnSubmit = context => async function onSubmit(e) {
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
        if (!state.phoneNumber.value) {
            formError = true;
            newState.phoneNumber.isError = true;
            newState.phoneNumber.errorMessage = 'Required';
        }

        setState(newState);
        if (!formError) {
            try {
                await axios.post('/UserGateway/SignUp', {
                    guestusername: context.username,
                    username: state.username.value,
                    password: state.password.value,
                    phone: state.phoneNumber.value
                });
                history.push('/login');
            }
            catch (e) {
                let errMsg = '';
                if (e.response && e.response.status === 400) {
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
        <GlobalContext.Consumer>
            {context => (
                <div>
                    <SimpleAlertDialog isShown={state.showErrorDialog} message={state.dialogErrorMessage} onClose={closeErrorDialog} />
                    <div className={formsClasses.center}>
                        <form className={formsClasses.form} onSubmit={makeOnSubmit(context)} noValidate autoComplete="off">
                            <h4 className={formsClasses.title}>Sign up</h4>
                            <div className={classes.formRow}>
                                <TextField label={'Username'} variant={'outlined'} className={classes.formInRowInput}
                                           required error={state.username.isError} helperText={state.username.errorMessage}
                                           onChange={handleChange("username")}/>
                                <PasswordField label={'Password'} variant={'outlined'} className={classes.formInRowInput}
                                               required error={state.password.isError} helperText={state.password.errorMessage}
                                               onChange={handleChange("password")} id={'input-password'}/>
                            </div>
                            <div className={classes.formRow}>
                                <TextField label={'Phone number'} variant={'outlined'} className={classes.formRow}
                                           required error={state.phoneNumber.isError} helperText={state.phoneNumber.errorMessage}
                                           onChange={handleChange("phoneNumber")}/>
                            </div>
                            <div className={clsx(classes.btnBlock, classes.formRow)}>
                                <Button variant="contained" color="primary" className={classes.btnCancel} onClick={onCancelClick}>Cancel</Button>
                                <Button type={"submit"} variant="contained" color="primary">Confirm</Button>
                            </div>
                        </form>
                    </div>
                </div>
            )}
        </GlobalContext.Consumer>
    );
}