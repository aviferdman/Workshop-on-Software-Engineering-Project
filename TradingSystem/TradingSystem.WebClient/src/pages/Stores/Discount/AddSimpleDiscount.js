import React from "react";
import './AddSimpleDiscount.css';
import { GlobalContext } from "../../../globalContext";
import * as api from "../../../api";

class AddSimpleDiscount extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            show: false
        }
    }

    showModal = () => {
        this.setState({ show: true });
    }

    hideModal = () => {
        this.setState({ show: false });
    }


    render() {
        return (
            <main className="items">
                <Modal show={this.state.show} handleClose={this.hideModal}  >

                    <div className="disc-check-line-grid">

                        <div>
                            <div className= "disc-col-grd-perm">
                                <div className="disc-text-props">
                                    <label>Discount Type</label>
                                </div>

                                <div>
                                    <select className="disc-input-props">
                                            <option value="Product">Product</option>
                                            <option value="Category">Category</option>
                                            <option value="Store">Store</option>
                                    </select>
                                </div>
                            </div>
                        </div>

                        <div>
                            <div className= "disc-col-grd-perm">
                                <div className="disc-text-props">
                                    <label>Condition</label>
                                </div>

                                <div>
                                    <select className="disc-input-props">
                                        <option value="Simple">Simple</option>
                                        <option value="Quantity">Quantity</option>
                                        <option value="Weight">Weight</option>
                                        <option value="Price">Price</option>
                                        <option value="Time">Time</option>
                                    </select>
                                </div>
                            </div>
                        </div>

                        <div>
                            <div className= "disc-col-grd-perm">
                                <div className="disc-text-props">
                                    <label>Percent</label>
                                </div>

                                <div >
                                    <input
                                        type="number"
                                        step="0.01"
                                        className="disc-input-props"
                                    />
                                </div>
                            </div>
                        </div>

                        <div>
                            <div className= "disc-col-grd-perm">
                                <div className="disc-text-props">
                                    <label>Product</label>
                                </div>

                                <div >
                                    <input
                                        type="text"
                                        className="disc-input-props"
                                    />
                                </div>
                            </div>
                        </div>

                        <div>
                            <div className= "disc-col-grd-perm">
                                <div className="disc-text-props">
                                    <label>Category</label>
                                </div>

                                <div >
                                    <input
                                        type="text"
                                        className="disc-input-props"
                                    />
                                </div>
                            </div>
                        </div>

                        <div>
                            <div className= "disc-col-grd-perm">
                                <div className="disc-text-props">
                                    <label>Min</label>
                                </div>

                                <div >
                                    <input
                                        type="number"
                                        step="0.01"
                                        className="disc-input-props"
                                    />
                                </div>
                            </div>
                        </div>

                        <div>
                            <div className= "disc-col-grd-perm">
                                <div className="disc-text-props">
                                    <label>Max</label>
                                </div>

                                <div >
                                    <input
                                        type="number"
                                        step="0.01"
                                        className="disc-input-props"
                                    />
                                </div>
                            </div>
                        </div>

                        <div>
                            <div className= "disc-col-grd-perm">
                                <div className="disc-text-props">
                                    <label>Start Date</label>
                                </div>

                                <div >
                                    <input
                                        type="date"
                                        className="disc-input-props"
                                        style={{width: "15rem" , height: "3rem" }}
                                    />
                                </div>
                            </div>
                        </div>

                        <div>
                            <div className= "disc-col-grd-perm">
                                <div className="disc-text-props">
                                    <label>End Date</label>
                                </div>

                                <div >
                                    <input
                                        type="date"
                                        className="disc-input-props"
                                        style={{width: "15rem" , height: "3rem" }}

                                    />
                                </div>
                            </div>
                        </div>

                    </div>

                </Modal>

                <button className= "store-products-button-view" onClick={this.showModal}> Add Simple Discount </button>
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
                    <h2 className="center-text">Add Simple Discount</h2>
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

AddSimpleDiscount.contextType = GlobalContext;
export default AddSimpleDiscount
