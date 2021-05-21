
import React from "react";
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
                            <text >Quantity</text>
                        </div>

                        <div >
                            <input
                                type="number"
                                placeholder="Quantity"
                                className="input-props"
                            />
                        </div>
                    </div>
                    <div className= "col-grd">
                        <div className="text-props">
                            <text >Price</text>
                        </div>

                        <div >
                            <input
                                type="number"
                                step="0.01"
                                placeholder="Price"
                                className="input-props"
                            />
                        </div>
                    </div>

                    <div className= "col-grd">
                        <div className="text-props">
                            <text >Weight</text>
                        </div>

                        <div >
                            <input
                                type="number"
                                step="0.01"
                                placeholder="Weight"
                                className="input-props"
                            />
                        </div>
                    </div>
                    <div className= "col-grd">
                        <div className="text-props">
                            <text >Category</text>
                        </div>

                        <div >
                            <input
                                type="text"
                                placeholder="Name"
                                className="input-props"
                            />
                        </div>
                    </div>

                </Modal>

                <button className= "store-products-button-view" onClick={this.showModal}> Add Product </button>
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
                    <h2 className="center-text">Add Product</h2>
                    {children}
                </div>


                    <div className="modal-buttons">
                        <button className="modal-buttons-props" onClick={handleClose} > Close </button>
                        <button className="modal-buttons-props" onClick={handleClose} > Add </button>
                    </div>


            </section>
        </div>
    );
};


export default AddProduct
