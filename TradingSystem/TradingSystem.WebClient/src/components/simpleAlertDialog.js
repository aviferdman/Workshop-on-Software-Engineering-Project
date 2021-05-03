import React from 'react';
import Button from '@material-ui/core/Button';
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import DialogContentText from '@material-ui/core/DialogContentText';

export default function SimpleAlertDialog(props) {
    return (
        <Dialog
            open={props.isShown}
            onClose={props.onClose}
            aria-labelledby="alert-dialog-title"
            aria-describedby="alert-dialog-description"
        >
            <DialogContent>
                <DialogContentText>
                    {props.message}
                </DialogContentText>
            </DialogContent>
            <DialogActions>
                <Button onClick={props.onClose} color="primary" autoFocus>
                    Close
                </Button>
            </DialogActions>
        </Dialog>
    );
}
