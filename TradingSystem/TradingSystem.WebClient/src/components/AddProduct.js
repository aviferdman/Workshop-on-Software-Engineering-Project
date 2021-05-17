
import React, { Component } from "react";
import './AddProduct.css';

class AddProduct extends React.Component {
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
                    <div className= "col-grd">
                        <div>
                            <text>Name</text>
                        </div>

                        <div >
                            <input
                                type="text"
                                placeholder="Name"
                                className="DsizeInput"
                            />
                        </div>
                    </div>

                    <div className= "col-grd">
                        <div>
                            <text>Quantity</text>
                        </div>

                        <div >
                            <input
                                type="number"
                                placeholder="Quantity"
                                className="DsizeInput"
                            />
                        </div>
                    </div>
                    <div className= "col-grd">
                        <div>
                            <text>Price</text>
                        </div>

                        <div >
                            <input
                                type="number"
                                step="0.01"
                                placeholder="Price"
                                className="DsizeInput"
                            />
                        </div>
                    </div>

                    <div className= "col-grd">
                        <div>
                            <text>Weight</text>
                        </div>

                        <div >
                            <input
                                type="number"
                                step="0.01"
                                placeholder="Weight"
                                className="DsizeInput"
                            />
                        </div>
                    </div>
                    <div className= "col-grd">
                        <div>
                            <text>Category</text>
                        </div>

                        <div >
                            <input
                                type="text"
                                placeholder="Name"
                                className="DsizeInput"
                            />
                        </div>
                    </div>

                </Modal>
                <button className= "store-products-button-view" onClick={this.showModal}> Add Product </button>
                <button className= "store-products-button-view" onClick={this.showModal}> Edit Product </button>
                <button className= "store-products-button-view" onClick={this.showModal}> Remove Product </button>
            </main>
        )
    }
}

const Modal = ({ handleClose, show, children }) => {
    const showHideClassName = show ? 'modal display-block' : 'modal display-none';

    return (
        <div className={showHideClassName}>
            <section className='modal-main'>
                {children}
                <button onClick={handleClose} > Close </button>
                <button onClick={handleClose} > Add </button>
            </section>
        </div>
    );
};


export default AddProduct
