
import React, { Component } from "react";
import './Purchase.css';

class Purchase extends React.Component {
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
                    <div className="check-line-grid">
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

                        <div className= "col-grd">
                            <div className="text-props">
                                <text >Card Num</text>
                            </div>

                            <div >
                                <input
                                    type="number"
                                    placeholder="Card"
                                    className="input-props"
                                />
                            </div>
                        </div>
                        <div className= "col-grd">
                            <div className="text-props">
                                <text >Year</text>
                            </div>

                            <div >
                                <input
                                    type="number"
                                    placeholder="Year"
                                    className="input-props"
                                />
                            </div>
                        </div>

                        <div className= "col-grd">
                            <div className="text-props">
                                <text >Month</text>
                            </div>

                            <div >
                                <input
                                    type="number"
                                    placeholder="Month"
                                    className="input-props"
                                />
                            </div>
                        </div>
                        <div className= "col-grd">
                            <div className="text-props">
                                <text >Card Owner</text>
                            </div>

                            <div >
                                <input
                                    type="text"
                                    placeholder="owner"
                                    className="input-props"
                                />
                            </div>
                        </div>

                        <div className= "col-grd">
                            <div className="text-props">
                                <text >ID</text>
                            </div>

                            <div >
                                <input
                                    type="number"
                                    placeholder="id"
                                    className="input-props"
                                />
                            </div>
                        </div>

                        <div className= "col-grd">
                            <div className="text-props">
                                <text >phone</text>
                            </div>

                            <div >
                                <input
                                    type="number"
                                    placeholder="phone"
                                    className="input-props"
                                />
                            </div>
                        </div>

                        <div className= "col-grd">
                            <div className="text-props">
                                <text >state</text>
                            </div>

                            <div >
                                <input
                                    type="text"
                                    placeholder="state"
                                    className="input-props"
                                />
                            </div>
                        </div>

                        <div className= "col-grd">
                            <div className="text-props">
                                <text >City</text>
                            </div>

                            <div >
                                <input
                                    type="text"
                                    placeholder="City"
                                    className="input-props"
                                />
                            </div>
                        </div>

                        <div className= "col-grd">
                            <div className="text-props">
                                <text >Street</text>
                            </div>

                            <div >
                                <input
                                    type="number"
                                    placeholder="Street"
                                    className="input-props"
                                />
                            </div>
                        </div>

                        <div className= "col-grd">
                            <div className="text-props">
                                <text >Apartment Num</text>
                            </div>

                            <div >
                                <input
                                    type="number"
                                    placeholder="AP Num"
                                    className="input-props"

                                />
                            </div>
                        </div>

                        <div className= "col-grd">
                            <div className="text-props">
                                <text >Zip</text>
                            </div>

                            <div >
                                <input
                                    type="number"
                                    placeholder="Zip"
                                    className="input-props"
                                />
                            </div>
                        </div>


                    </div>


                </Modal>

                <button className= "store-products-button-view" onClick={this.showModal}> Purchase </button>
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
                    <h2 className="center-text">Purchase</h2>
                    {children}
                </div>


                <div className="modal-buttons">
                    <button className="modal-buttons-props" onClick={handleClose} > Close </button>
                    <button className="modal-buttons-props" onClick={handleClose} > Purchase </button>
                </div>


            </section>
        </div>
    );
};


export default Purchase
