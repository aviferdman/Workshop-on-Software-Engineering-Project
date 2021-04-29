import React from "react";
import { useHistory } from "react-router-dom";
import clsx from 'clsx';
import {
    Button,
    FormControl,
    IconButton,
    InputAdornment,
    InputLabel,
    makeStyles,
    OutlinedInput,
    TextField
} from "@material-ui/core";
import {Visibility, VisibilityOff} from "@material-ui/icons";
import './Login.scss'
import {useTitle} from "../App";
import PasswordField from "../components/passwordFields";

const useStyles = makeStyles((theme) => ({
    root: {
        '& .MuiTextField-root': {
            margin: theme.spacing(1),
            width: '25ch',
        },
    },
    margin: {
        margin: theme.spacing(1)
    },
    textField: {
        width: '25ch',
        display: 'block',
    },
    formRow: {
        width: '216px',
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
    const classes = useStyles();
    const [values, setValues] = React.useState({
        username: '',
        password: '',
        errorMessage: '',
    });

    const handleChange = (prop) => (event) => {
        setValues({ ...values, [prop]: event.target.value });
    };

    const history = useHistory();
    const onSignupClick = e => {
        history.push('/signup');
    };

    let errorMessageElement = null;
    let blockHeight = '300px';
    if (values.errorMessage) {
        errorMessageElement = <div className={clsx(classes.errorMessage, classes.formRow)}>
            <label> { values.errorMessage } </label>
        </div>
        blockHeight = '320px';
    }

    useTitle('Login');

    return (
        <div className="center-block" style={{height: blockHeight}}>
            <div className="vertical-center-block">
                <h4 className={classes.title}>Login</h4>
                <form className={classes.root} noValidate autoComplete="off">
                    <TextField value={values.username} className={classes.textField} label="Username" variant="outlined" onChange={handleChange('username')} />
                    <PasswordField value={values.password}
                                   onChange={handleChange('password')}
                                   label="Password"
                                   className={clsx(classes.margin, classes.textField)}
                                   variant="outlined"
                                   id="outlined-adornment-password" />
                    {errorMessageElement}
                    <div className={classes.formRow}>
                        <Button variant="contained" color="primary" className={classes.btnLogin}>Login</Button>
                        <Button variant="contained" color="primary" onClick={onSignupClick}>Sign up</Button>
                    </div>
                    <div className={clsx(classes.formRow, classes.topMargin)}>
                        <Button variant="contained" style={{textTransform: 'none'}}>Continue as guest</Button>
                    </div>
                </form>
            </div>
        </div>
    );
}
