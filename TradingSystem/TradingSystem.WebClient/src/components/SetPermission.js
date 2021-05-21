
import React from "react";
import './SetPermission.css';

class SetPermission extends React.Component {
    state = { show: false }

    showModal = () => {
        this.setState({ show: true });
    }

    hideModal = () => {
        this.setState({ show: false });
    }

    render() {
        return (
            <main className="items">
                <Modal show={this.state.show} handleClose={this.hideModal} >
                    <div className="rows-grid">
                        <div>
                            <div className= "col-grd">
                                <div className="text-props">
                                    <text >Name</text>
                                </div>

                                <div >
                                    <input
                                        type="text"
                                        placeholder="Name"
                                        className="input-props"
                                    />
                                </div>
                            </div>
                        </div>


                        <div className="check-line-grid">
                            <div>
                                <div className= "col-grd-perm">
                                    <div className="text-props">
                                        <text >Add Product</text>
                                    </div>

                                    <div >
                                        <input
                                            type="checkbox"
                                            className="input-props"
                                        />
                                    </div>
                                </div>
                            </div>

                            <div>
                                <div className= "col-grd-perm">
                                    <div className="text-props">
                                        <text >Appoint Manager</text>
                                    </div>

                                    <div >
                                        <input
                                            type="checkbox"
                                            className="input-props"
                                        />
                                    </div>
                                </div>
                            </div>

                            <div>
                                <div className= "col-grd-perm">
                                    <div className="text-props">
                                        <text >Remove Product</text>
                                    </div>

                                    <div >
                                        <input
                                            type="checkbox"
                                            className="input-props"
                                        />
                                    </div>
                                </div>
                            </div>

                            <div>
                                <div className= "col-grd-perm">
                                    <div className="text-props">
                                        <text >Get Personell Info</text>
                                    </div>

                                    <div >
                                        <input
                                            type="checkbox"
                                            className="input-props"
                                        />
                                    </div>
                                </div>

                            </div>

                            <div>
                                <div className= "col-grd-perm">
                                    <div className="text-props">
                                        <text >Edit Product</text>
                                    </div>

                                    <div >
                                        <input
                                            type="checkbox"
                                            className="input-props"
                                        />
                                    </div>
                                </div>

                            </div>

                            <div>
                                <div className= "col-grd-perm">
                                    <div className="text-props">
                                        <text >Get Shop History</text>
                                    </div>

                                    <div >
                                        <input
                                            type="checkbox"
                                            className="input-props"
                                        />
                                    </div>
                                </div>

                            </div>

                            <div>
                                <div className= "col-grd-perm">
                                    <div className="text-props">
                                        <text >Edit Permissions</text>
                                    </div>

                                    <div >
                                        <input
                                            type="checkbox"
                                            className="input-props"
                                        />
                                    </div>
                                </div>

                            </div>

                            <div>
                                <div className= "col-grd-perm">
                                    <div className="text-props">
                                        <text >Close Shop</text>
                                    </div>

                                    <div >
                                        <input
                                            type="checkbox"
                                            className="input-props"
                                        />
                                    </div>
                                </div>

                            </div>

                            <div>
                                <div className= "col-grd-perm">
                                    <div className="text-props">
                                        <text >Edit Discount</text>
                                    </div>

                                    <div >
                                        <input
                                            type="checkbox"
                                            className="input-props"
                                        />
                                    </div>
                                </div>

                            </div>

                            <div>
                                <div className= "col-grd-perm">
                                    <div className="text-props">
                                        <text >Edit Policy</text>
                                    </div>

                                    <div >
                                        <input
                                            type="checkbox"
                                            className="input-props"
                                        />
                                    </div>
                                </div>

                            </div>

                            <div>
                                <div className= "col-grd-perm">
                                    <div className="text-props">
                                        <text >Bid Request</text>
                                    </div>

                                    <div >
                                        <input
                                            type="checkbox"
                                            className="input-props"
                                        />
                                    </div>
                                </div>

                            </div>

                        </div>

                    </div>


                </Modal>

                <button className= "store-products-button-view" onClick={this.showModal}> Update Permissions </button>
            </main>
        )
    }
}

const Modal = ({ handleClose, show, children }) => {
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
                    <button className="modal-buttons-props" onClick={handleClose} > Update </button>
                </div>


            </section>
        </div>
    );
};


export default SetPermission
