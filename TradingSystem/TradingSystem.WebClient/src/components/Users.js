import React, {Component} from 'react';
import * as AiIcons from "react-icons/ai";
import SetPermission from "./SetPermission";
import {GlobalContext} from "../globalContext";
import * as api from '../api'
import {alertRequestError_default} from "../utils";

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

    render() {
        return (
            <div className='store-staff-users'>
                <ul className = "products">
                    {this.props.staff.map((user) => (
                        <li key={user.username}>
                            <div className={user.role !== 'founder' ? 'control-buttons-staff' : 'control-buttons-staff margin-no-controls'}>
                                {user.role !== 'founder' ? (
                                    <button className="exit-button" onClick={this.onRemove(user)}>
                                        <AiIcons.AiOutlineClose />
                                    </button>
                                ) : null}
                                {user.role === 'manager' ? (
                                    <SetPermission storeId={this.props.storeId} user={user} />
                                ) : null}
                            </div>
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