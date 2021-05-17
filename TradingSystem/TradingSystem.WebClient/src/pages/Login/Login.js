import React from "react";
import { useHistory } from "react-router-dom";
import clsx from 'clsx';
import {
    Button,
    makeStyles,
    TextField
} from "@material-ui/core";
import './Login.scss'
import {useTitle} from "../../App";
import PasswordField from "../../components/passwordFields";
import FormFieldInfo from "../../formFieldInfo";
import axios from "axios";
import useFormsStyles from "../../style/forms";
import SimpleAlertDialog from "../../components/simpleAlertDialog";
import {GlobalContext} from "../../globalContext";

const useStyles = makeStyles((theme) => ({
    form: {
      width: '300px'
    },
    margin: {
        margin: theme.spacing(1)
    },
    textField: {
        width: '25ch',
    },
    topMargin: {
        marginTop: theme.spacing(1)
    },
    btnLogin: {
        marginRight: theme.spacing(1),
    },
    title: {
        fontSize: '1.5rem',
        marginTop: '0px',
        marginLeft: theme.spacing(1),
        marginBottom: theme.spacing(2)
    },
    errorMessage: {
        color: "red",
        marginBottom: theme.spacing(1)
    }
}));

export default function LoginPage() {
    useTitle('Login');

    const classes = useStyles();
    const formsClasses = useFormsStyles();

    const [state, setState] = React.useState({
        username: new FormFieldInfo(''),
        password: new FormFieldInfo(''),
        errorMessage: '',
        showErrorDialog: false
    });

    const history = useHistory();
    const onSignupClick = e => {
        history.push('/signup');
    };

    const closeErrorDialog = () => {
        setState({
           ...state,
           showErrorDialog: false,
           errorMessage: ''
        });
    };

    const handleChange = prop => e => {
        let newState = {
            ...state
        };
        newState[prop].value = e.target.value;
        setState(newState);
    };

    let submitting = false;
    const makeOnSubmit = context => async function onSubmit(e) {
        e.preventDefault();
        if (submitting) {
            return;
        }

        submitting = true;
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

        setState(newState);
        if (!formError) {
            try {
                let response = await axios.post('/UserGateway/Login', {
                    guestusername: context.username,
                    username: state.username.value,
                    password: state.password.value,
                });
                if (response.data === "success") {
                    context.setUsername(state.username.value, true);
                    context.setWebSocket(state.username.value);
                    history.push('/home');
                }
                else {
                    setState({
                        ...state,
                        errorMessage: 'Error: ' + response.data,
                        showErrorDialog: true
                    });
                }
            }
            catch (e) {
                let errMsg = '';
                let showErrDialog = false;
                if (e.response && e.response.status === 400) {
                    if (e.response.data.startsWith('user is already logged in')) {
                        errMsg = 'Already logged-in';
                    }
                    else if (e.response.data.startsWith('the password doesn\'t match')) {
                        errMsg = 'Incorrect password'
                    }
                    else if (e.response.data.startsWith('username: ' + state.username.value + ' doesn\'t exist')) {
                        errMsg = 'Username doesn\'t exist'
                    }
                }
                if (!errMsg) {
                    let errMsgBuilder = '';
                    if (e.response && e.response.data) {
                        errMsgBuilder += e.response.data;
                    }
                    if (e.message) {
                        if (errMsgBuilder) {
                            errMsgBuilder += ', ';
                        }
                        errMsgBuilder += e.message;
                    }
                    errMsg = 'Unknown error occurred';
                    if (errMsgBuilder) {
                        errMsg += ': ' + errMsgBuilder;
                    }
                    showErrDialog = true;
                }
                setState({
                    ...state,
                    errorMessage: errMsg,
                    showErrorDialog: showErrDialog
                });
            }
        }
        submitting = false;
    }

    let errorMessageElement = null;
    if (state.errorMessage && !state.showErrorDialog) {
        errorMessageElement = (
            <div className={classes.errorMessage}>
                <label>{state.errorMessage}</label>
            </div>
        );
    }

    return (
        <GlobalContext.Consumer>
        {context => (
            <div>
                <SimpleAlertDialog isShown={state.showErrorDialog} message={state.errorMessage} onClose={closeErrorDialog} />
                <div className={formsClasses.center}>
                    <form className={clsx(formsClasses.form, classes.form)} onSubmit={makeOnSubmit(context)} noValidate autoComplete="off">
                        <h4 className={formsClasses.title}>Login</h4>
                        <TextField value={state.username.value} onChange={handleChange('username')}
                                   error={state.username.isError} helperText={state.username.errorMessage}
                                   label="Username" variant="outlined" className={clsx(classes.margin, classes.textField)} />
                        <PasswordField value={state.password.value} onChange={handleChange('password')}
                                       error={state.password.isError} helperText={state.password.errorMessage}
                                       label="Password" variant="outlined" className={clsx(classes.margin, classes.textField)}
                                       id="outlined-adornment-password" />
                        {errorMessageElement}
                        <div className={classes.topMargin}>
                            <Button variant="contained" color="primary" type='submit' className={classes.btnLogin}>Login</Button>
                            <Button variant="contained" color="primary" onClick={onSignupClick}>Sign up</Button>
                        </div>
                        <div className={classes.topMargin}>
                            <Button variant="contained" style={{textTransform: 'none'}}>Continue as guest</Button>
                        </div>
                    </form>
                </div>
            </div>
        )}
        </GlobalContext.Consumer>
    );
}
