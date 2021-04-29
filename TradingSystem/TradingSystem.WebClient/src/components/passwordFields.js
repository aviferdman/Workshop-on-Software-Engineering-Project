import React, {Component} from "react";
import {FormControl, IconButton, InputAdornment, InputLabel, OutlinedInput} from "@material-ui/core";
import {Visibility, VisibilityOff} from "@material-ui/icons";

export default class PasswordField extends Component {
    constructor(props) {
        super(props);
        this.state = {
            showPassword: false
        };

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
            <FormControl variant={this.props.variant} className={this.props.className}>
                <InputLabel htmlFor={this.props.id}>{this.props.label}</InputLabel>
                <OutlinedInput
                    id={this.props.id}
                    type={this.state.showPassword ? 'text' : 'password'}
                    value={this.props.value}
                    onChange={this.props.onChange}
                    label={this.props.label} /* this only causes enough spacing when the actual label is moved above the input field */
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
            </FormControl>
        )
    }
}