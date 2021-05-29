import React, {Component} from "react";
import {FormControl, FormHelperText, IconButton, InputAdornment, InputLabel, OutlinedInput} from "@material-ui/core";
import {Visibility, VisibilityOff} from "@material-ui/icons";

export default class PasswordField extends Component {
    constructor(props) {
        super(props);
        this.state = {
            showPassword: true
        };

        this.formControlProps = {...props};
        delete this.formControlProps.id;
        delete this.formControlProps.label;
        delete this.formControlProps.value;
        delete this.formControlProps.onChange;
        delete this.formControlProps.error;
        delete this.formControlProps.helperText;
        delete this.formControlProps.className;
        delete this.formControlProps.variant;

        this.handleClickShowPassword = this.handleClickShowPassword.bind(this);
        this.handleMouseDownPassword = this.handleMouseDownPassword.bind(this);
    }

    handleClickShowPassword = () => {
        this.setState({
            showPassword: !this.state.showPassword
        });
    };

    handleMouseDownPassword = e => {
        e.preventDefault();
    };

    render() {
        return (
            <FormControl variant={this.props.variant} className={this.props.className} {...this.formControlProps}>
                <InputLabel htmlFor={this.props.id} error={this.props.error}>{this.props.label}</InputLabel>
                <OutlinedInput
                    id={this.props.id}
                    type={this.state.showPassword ? 'text' : 'password'}
                    value={this.props.value}
                    onChange={this.props.onChange}
                    label={this.props.label} /* this only causes enough spacing when the actual label is moved above the input field */
                    error={this.props.error}
                    endAdornment={
                        <InputAdornment position="end">
                            <IconButton
                                aria-label="toggle password visibility"
                                onClick={this.handleClickShowPassword}
                                onMouseDown={this.handleMouseDownPassword}
                                edge="end"
                            >
                                {this.state.showPassword ? <Visibility /> : <VisibilityOff />}
                            </IconButton>
                        </InputAdornment>
                    }
                    labelWidth={70}
                />
                {!!this.props.error && (
                    <FormHelperText error id={this.props.id + "_errMsg"}>
                        {this.props.helperText}
                    </FormHelperText>
                )}
            </FormControl>
        )
    }
}