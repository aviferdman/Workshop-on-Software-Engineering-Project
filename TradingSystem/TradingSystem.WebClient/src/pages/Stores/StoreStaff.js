import React, {Component} from 'react';
import './StoreStaff.css';
import Users from "../../components/Users";
import AddManager from "../../components/AddManager";
import AddOwner from "../../components/AddOwner";
import {GlobalContext} from "../../globalContext";
import * as api from "../../api";
import * as util from "../../utils";
import {alertRequestError_default} from "../../utils";
import StoreRestrictedComponentCustom from "../../components/StoreRestrictedComponentCustom";

export class StoreStaff extends Component {
    constructor(props) {
        super(props);
        this.state = {
            name: "",
            staff: [],
            myPermissions: {
                role: 'guest',
                actions: {},
            },
        };
        this.storeId = this.props.match.params.storeId;
    }

    async componentDidMount() {
        await Promise.all([
            this.fetchStaff(),
            this.fetchMyStorePermissions(),
        ]);
    }

    async fetchStaff() {
        return api.stores.permissions.workersDetails(this.context.username, this.storeId)
            .then(staff => {
                this.setState({
                    staff: staff,
                });
            }, alertRequestError_default);
    }

    async fetchMyStorePermissions() {
        await api.stores.permissions.mine(this.context.username, this.storeId)
            .then(permissions => {
                this.setState({
                    myPermissions: {
                        role: permissions.role,
                        actions: util.arrayToHashset(permissions.permissions),
                    },
                });
            }, alertRequestError_default);
    }

    onUserAdd = user => {
        this.state.staff.push(user);
        this.setState({
            ...this.state
        });
    }

    onUserRemove = async user => {
        if (user.role === 'manager') {
            this.setState({
                staff: this.state.staff.filter(u => {
                    return u.username !== user.username;
                }),
            });
        }
        else {
            await this.fetchStaff();
        }
    };

    render() {
        return (
            <main className="store-products-main-conatiner-staff">

                <div>
                    <Users staff={this.state.staff}
                           storeId={this.storeId}
                           myPermissions={this.state.myPermissions}
                           onRemove={this.onUserRemove} />
                </div>

                <StoreRestrictedComponentCustom
                    permissions={this.state.myPermissions}
                    allowedActions={[]}
                    render={() => (
                        <div className="bottom-row-staff">
                            <div>
                                <AddManager storeId={this.storeId} onSuccess={this.onUserAdd} />
                            </div>

                            <div>
                                <AddOwner storeId={this.storeId} onSuccess={this.onUserAdd} />
                            </div>
                        </div>
                    )} />

            </main>
        )
    }
}

StoreStaff.contextType = GlobalContext;