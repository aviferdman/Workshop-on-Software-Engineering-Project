import React from "react";
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
    formButtonsBlock: {
        display: 'flex',
        width: '36%',
    },
    btnLogin: {
        marginRight: '10px',
    }
}));

export default function LoginPage() {
    const classes = useStyles();
    const [values, setValues] = React.useState({
        username: '',
        password: '',
        showPassword: false,
    });

    const handleChange = (prop) => (event) => {
        setValues({ ...values, [prop]: event.target.value });
    };

    const handleClickShowPassword = () => {
        setValues({ ...values, showPassword: !values.showPassword });
    };

    const handleMouseDownPassword = (event) => {
        event.preventDefault();
    };

    return (
        <div className="center-block">
            <div className="vertical-center-block">
                <form className={classes.root} noValidate autoComplete="off">
                    <TextField value={values.username} className={classes.textField} label="Username" variant="outlined" onChange={handleChange('username')} />
                    <FormControl className={clsx(classes.margin, classes.textField)} variant="outlined">
                        <InputLabel htmlFor="outlined-adornment-password">Password</InputLabel>
                        <OutlinedInput
                            id="outlined-adornment-password"
                            type={values.showPassword ? 'text' : 'password'}
                            value={values.password}
                            onChange={handleChange('password')}
                            endAdornment={
                                <InputAdornment position="end">
                                    <IconButton
                                        aria-label="toggle password visibility"
                                        onClick={handleClickShowPassword}
                                        onMouseDown={handleMouseDownPassword}
                                        edge="end"
                                    >
                                        {values.showPassword ? <Visibility /> : <VisibilityOff />}
                                    </IconButton>
                                </InputAdornment>
                            }
                            labelWidth={70}
                        />
                    </FormControl>
                    <div className={classes.formButtonsBlock}>
                        <Button variant="contained" color="primary" className={classes.btnLogin}>Login</Button>
                        <Button variant="contained" color="primary">Sign up</Button>
                    </div>
                </form>
            </div>
        </div>
    );
}
