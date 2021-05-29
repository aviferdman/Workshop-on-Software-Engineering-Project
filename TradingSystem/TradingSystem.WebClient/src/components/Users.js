import React, {Component} from 'react';
import * as AiIcons from "react-icons/ai";
import SetPermission from "./SetPermission";
import {GlobalContext} from "../globalContext";
import * as api from '../api'
import {alertRequestError_default} from "../utils";
import StoreRestrictedComponentCustom from "./StoreRestrictedComponentCustom";

class Users extends Component {
    onRemove = user => e => {
        e.preventDefault();
        let fApi = null;
        let promise = null;
        if (user.role === 'manager') {
            fApi = api.stores.permissions.removeManager;
        }
        else if (user.role === 'owner') {
            fApi = api.stores.permissions.removeOwner;
        }
        if (fApi != null) {
            promise = fApi(this.context.username, this.props.storeId, user.username)
                .then(async () => {
                    await this.props.onRemove(user);
                }, alertRequestError_default);
        }
        return promise;
    }

    renderControlButtons = user => {
        let classes = 'control-buttons-staff';
        let removeButton = null;
        let changePermissionsButton = null;

        if (user.role === 'founder') {
            classes += ' margin-no-controls';
        }
        else {
            removeButton = (
                <button className="exit-button" onClick={this.onRemove(user)}>
                    <AiIcons.AiOutlineClose />
                </button>
            );
        }
        if (user.role === 'manager') {
            changePermissionsButton = (<SetPermission storeId={this.props.storeId} user={user} />);
        }

        return (
            <div className={classes}>
                {removeButton}
                {changePermissionsButton}
            </div>
        );
    }

    render() {
        return (
            <div className='store-staff-users'>
                <ul className = "products">
                    {this.props.staff.map((user) => (
                        <li key={user.username}>
                            <StoreRestrictedComponentCustom
                                permissions={this.props.myPermissions}
                                allowedActions={[]}
                                render={() => this.renderControlButtons(user)} />
                            <div className = "user">
                                <a href={"#" + user.username}>
                                    <p className= "userName">{user.username}</p>
                                </a>
                                <div>
                                    <p> {user.role} </p>
                                    <p> {user.phone} </p>
                                </div>
                            </div>
                        </li>
                    ))}
                </ul>
            </div>
        );
    }
}

Users.contextType = GlobalContext;
export default Users;