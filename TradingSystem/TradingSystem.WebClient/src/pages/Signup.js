import {useTitle} from "../App";
import {useHistory} from "react-router-dom";
import {Button, makeStyles, TextField} from "@material-ui/core";
import PasswordField from "../components/passwordFields";
import React from "react";
import clsx from "clsx";

export default function Signup() {
    useTitle('Sign up');
    const history = useHistory();

    const useStyles = makeStyles((theme) => ({
        center: {
            display: 'flex',
            justifyContent: 'center',
            alignItems: 'center',
            height: '100vh'
        },
        form: {
            border: '2px solid gray',
            // width: '300px',
            // height: '300px',
            padding: theme.spacing(1)
        },
        title: {
            fontSize: '1.5rem',
            marginTop: '0px',
            marginBottom: theme.spacing(1),
            alignSelf: 'flex-start'
        },
        formRow: {
            marginTop: theme.spacing(1),
            alignSelf: 'flex-start'
        },
        formInRowInput: {
            marginRight: theme.spacing(1)
        },
        btnSubmit: {
            alignSelf: 'flex-end'
        }
    }));
    const classes = useStyles();

    return (
        <div className={classes.center}>
            <form className={classes.form}>
                <h4 className={classes.title}>Sign up</h4>
                <TextField label={'Username'} variant={'outlined'} className={classes.formRow} />
                <div className={classes.formRow}>
                    <PasswordField label={'Password'} variant={'outlined'} className={classes.formInRowInput} id={'input-password'} />
                    <PasswordField label={'Confirm password'} variant={'outlined'} id={'input-password-confirm'} />
                </div>
                <TextField label={'Phone number'} variant={'outlined'} className={classes.formRow} />
                <div className={classes.formRow}>
                    <TextField label={'State'} variant={'outlined'} className={classes.formInRowInput} />
                    <TextField label={'City'} variant={'outlined'} />
                </div>
                <div className={classes.formRow}>
                    <TextField label={'Street'} variant={'outlined'} className={classes.formInRowInput} />
                    <TextField label={'Apartment number'} variant={'outlined'} />
                </div>
                <div className={clsx(classes.btnSubmit, classes.formRow)}>
                    <Button variant="contained" color="primary">Confirm</Button>
                </div>
            </form>
        </div>
    )
}