import React from "react";
import './AddManager.css';
import FormFieldInfo from "../formsUtil/formFieldInfo";
import { alertRequestError_default } from "../utils";
import * as api from "../api";
import { GlobalContext } from "../globalContext";

class AddOwner extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            show: false,
            nameField: new FormFieldInfo(),
        }
    }

    showModal = () => {
        this.setState({ show: true });
    }

    hideModal = () => {
        this.setState({ show: false });
    }

    onInputChange = e => {
        e.preventDefault();
        if (!this.state.nameField.trySetValueFromEvent(e)) {
            return;
        }
        this.setState({
            ...this.state
        });
    }

    onConfirm = e => {
        e.preventDefault();
        if (!this.state.nameField.validate()) {
            alert('Please fill all fields');
            return;
        }

        let assignee = this.state.nameField.value;
        api.stores.permissions.appointOwner(this.context.username, this.props.storeId, assignee)
            .then(() => {
                this.setState({
                    show: false,
                    nameField: new FormFieldInfo(),
                });
                this.props.onSuccess({
                    username: assignee,
                    role: 'owner',
                });
            }, alertRequestError_default);
    }

    render() {
        return (
            <main className="items">
                <Modal show={this.state.show} handleClose={this.hideModal} handleConfirm={this.onConfirm} >
                    <div className= "col-grd">
                        <div className="text-props">
                            <label >UserName</label>
                        </div>

                        <div >
                            <input
                                type="text"
                                placeholder="UserName"
                                className="input-props"
                                required
                                value={this.state.nameField.value}
                                onChange={this.onInputChange}
                            />
                        </div>
                    </div>

                </Modal>

                <button className= "store-products-button-view" onClick={this.showModal}> Add Owner </button>
            </main>
        )
    }
}

const Modal = ({ handleClose, handleConfirm, show, children }) => {
    const showHideClassName = show ? 'modal display-block' : 'modal display-none';

    return (
        <div className={showHideClassName}>
            <section className='modal-main'>

                <div className="lines-props">
                    <h2 className="center-text">Add Owner</h2>
                    {children}
                </div>


                <div className="modal-buttons">
                    <button className="modal-buttons-props" onClick={handleClose} > Close </button>
                    <button className="modal-buttons-props" onClick={handleConfirm} > Add </button>
                </div>


            </section>
        </div>
    );
};

AddOwner.contextType = GlobalContext;
export default AddOwner
