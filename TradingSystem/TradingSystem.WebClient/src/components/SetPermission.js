import React from "react";
import './SetPermission.css';
import { alertRequestError_default } from "../utils";
import * as api from "../api";
import { GlobalContext } from "../globalContext";
import * as AiIcons from "react-icons/ai";
import * as util from "../utils";

class SetPermission extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            show: false,
            permissions: {},
        };

        if (props.user.permissions != null) {
            this.state.permissions = this.setPermissionsFromArray(this.props.user.permissions);
        }
    }

    showModal = () => {
        this.setState({ show: true });
    }

    hideModal = () => {
        this.setState({ show: false });
    }

    isChecked = permission => {
        return !!this.state.permissions[permission];
    }

    async componentDidMount() {
        let promise = null;
        if (this.props.user.permissions == null) {
            promise = api.stores.permissions.workerSpecificDetails(
                this.context.username,
                this.props.storeId,
                this.props.user.username
            ).then(workerDetails => {
                this.props.user.permissions = workerDetails.permissions;
                let permissions = this.setPermissionsFromArray(workerDetails.permissions);
                this.setState({
                    permissions: permissions,
                });
            }, alertRequestError_default);
        }
        return promise;
    }

    setPermissionsFromArray = permissions => {
        this.props.user.permissionsMap = util.arrayToHashset(permissions);
        return this.props.user.permissionsMap;
    }

    onCheckboxCheckedChange = e => {
        let target = e.target;
        if (target.checked) {
            this.state.permissions[target.value] = {};
        }
        else {
            delete this.state.permissions[target.value];
        }
        this.setState({
            ...this.state
        });
    }

    onConfirm = e => {
        e.preventDefault();

        let permissions = Object.keys(this.state.permissions);
        api.stores.permissions.updateManagerPermissions(
            this.context.username,
            this.props.storeId,
            this.props.user.username,
            permissions,
        ).then(() => {
            this.setState({
                show: false,
            });
            this.props.user.permissions = permissions;
        }, alertRequestError_default);
    }

    render() {
        return (
            <div style={{
                display: 'inline'
            }}>
                <Modal show={this.state.show} handleClose={this.hideModal} handleConfirm={this.onConfirm} >
                    <div className="rows-grid">
                        <div>
                            <div className= "col-grd">
                                <div className="text-props">
                                    <label>Name</label>
                                </div>

                                <div >
                                    <input
                                        type="text"
                                        placeholder="Name"
                                        className="input-props"
                                        readOnly
                                        value={this.props.user.username}
                                    />
                                </div>
                            </div>
                        </div>

                        <div className="check-line-grid">
                            <div>
                                <div className= "col-grd-perm">
                                    <div className="text-props">
                                        <label >Add Product</label>
                                    </div>

                                    <div >
                                        <input
                                            type="checkbox"
                                            className="input-props"
                                            value={api.data.stores.permissions.addProduct}
                                            checked={this.isChecked(api.data.stores.permissions.addProduct)}
                                            onChange={this.onCheckboxCheckedChange}
                                        />
                                    </div>
                                </div>
                            </div>

                            <div>
                                <div className= "col-grd-perm">
                                    <div className="text-props">
                                        <label>Appoint Manager</label>
                                    </div>

                                    <div >
                                        <input
                                            type="checkbox"
                                            className="input-props"
                                            value={api.data.stores.permissions.appointManger}
                                            checked={this.isChecked(api.data.stores.permissions.appointManger)}
                                            onChange={this.onCheckboxCheckedChange}
                                        />
                                    </div>
                                </div>
                            </div>

                            <div>
                                <div className= "col-grd-perm">
                                    <div className="text-props">
                                        <label>Remove Product</label>
                                    </div>

                                    <div >
                                        <input
                                            type="checkbox"
                                            className="input-props"
                                            value={api.data.stores.permissions.removeProduct}
                                            checked={this.isChecked(api.data.stores.permissions.removeProduct)}
                                            onChange={this.onCheckboxCheckedChange}
                                        />
                                    </div>
                                </div>
                            </div>

                            <div>
                                <div className= "col-grd-perm">
                                    <div className="text-props">
                                        <label>Get Personnel Info</label>
                                    </div>

                                    <div >
                                        <input
                                            type="checkbox"
                                            className="input-props"
                                            value={api.data.stores.permissions.getPersonnelInfo}
                                            checked={this.isChecked(api.data.stores.permissions.getPersonnelInfo)}
                                            onChange={this.onCheckboxCheckedChange}
                                        />
                                    </div>
                                </div>

                            </div>

                            <div>
                                <div className= "col-grd-perm">
                                    <div className="text-props">
                                        <label>Edit Product</label>
                                    </div>

                                    <div >
                                        <input
                                            type="checkbox"
                                            className="input-props"
                                            value={api.data.stores.permissions.editProduct}
                                            checked={this.isChecked(api.data.stores.permissions.editProduct)}
                                            onChange={this.onCheckboxCheckedChange}
                                        />
                                    </div>
                                </div>

                            </div>

                            <div>
                                <div className= "col-grd-perm">
                                    <div className="text-props">
                                        <label>Get Shop History</label>
                                    </div>

                                    <div >
                                        <input
                                            type="checkbox"
                                            className="input-props"
                                            value={api.data.stores.permissions.getShopHistory}
                                            checked={this.isChecked(api.data.stores.permissions.getShopHistory)}
                                            onChange={this.onCheckboxCheckedChange}
                                        />
                                    </div>
                                </div>

                            </div>

                            <div>
                                <div className= "col-grd-perm">
                                    <div className="text-props">
                                        <label>Edit Permissions</label>
                                    </div>

                                    <div >
                                        <input
                                            type="checkbox"
                                            className="input-props"
                                            value={api.data.stores.permissions.editPermissions}
                                            checked={this.isChecked(api.data.stores.permissions.editPermissions)}
                                            onChange={this.onCheckboxCheckedChange}
                                        />
                                    </div>
                                </div>

                            </div>

                            <div>
                                <div className= "col-grd-perm">
                                    <div className="text-props">
                                        <label>Close Shop</label>
                                    </div>

                                    <div >
                                        <input
                                            type="checkbox"
                                            className="input-props"
                                            value={api.data.stores.permissions.closeShop}
                                            checked={this.isChecked(api.data.stores.permissions.closeShop)}
                                            onChange={this.onCheckboxCheckedChange}
                                        />
                                    </div>
                                </div>

                            </div>

                            <div>
                                <div className= "col-grd-perm">
                                    <div className="text-props">
                                        <label>Edit Discount</label>
                                    </div>

                                    <div >
                                        <input
                                            type="checkbox"
                                            className="input-props"
                                            value={api.data.stores.permissions.editDiscount}
                                            checked={this.isChecked(api.data.stores.permissions.editDiscount)}
                                            onChange={this.onCheckboxCheckedChange}
                                        />
                                    </div>
                                </div>

                            </div>

                            <div>
                                <div className= "col-grd-perm">
                                    <div className="text-props">
                                        <label>Edit Policy</label>
                                    </div>

                                    <div >
                                        <input
                                            type="checkbox"
                                            className="input-props"
                                            value={api.data.stores.permissions.editPolicy}
                                            checked={this.isChecked(api.data.stores.permissions.editPolicy)}
                                            onChange={this.onCheckboxCheckedChange}
                                        />
                                    </div>
                                </div>

                            </div>

                            <div>
                                <div className= "col-grd-perm">
                                    <div className="text-props">
                                        <label>Bid Requests</label>
                                    </div>

                                    <div >
                                        <input
                                            type="checkbox"
                                            className="input-props"
                                            value={api.data.stores.permissions.bidRequests}
                                            checked={this.isChecked(api.data.stores.permissions.bidRequests)}
                                            onChange={this.onCheckboxCheckedChange}
                                        />
                                    </div>
                                </div>

                            </div>

                        </div>

                    </div>
                </Modal>

                <button className="pointer-cursor" onClick={this.showModal}>
                    <AiIcons.AiFillEdit />
                </button>
            </div>
        )
    }
}

const Modal = ({ handleClose, handleConfirm, show, children }) => {
    const showHideClassName = show ? 'modal display-block' : 'modal display-none';

    return (
        <div className={showHideClassName}>
            <section className='modal-main'>

                <div className="lines-props">
                    <h2 className="center-text">Update Permissions</h2>
                    {children}
                </div>


                <div className="modal-buttons">
                    <button className="modal-buttons-props" onClick={handleClose} > Close </button>
                    <button className="modal-buttons-props" onClick={handleConfirm} > Update </button>
                </div>


            </section>
        </div>
    );
};

SetPermission.contextType = GlobalContext;
export default SetPermission
