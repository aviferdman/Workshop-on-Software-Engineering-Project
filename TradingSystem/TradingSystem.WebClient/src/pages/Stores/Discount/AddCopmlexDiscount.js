import React from "react";
import './AddSimpleDiscount.css';
import { GlobalContext } from "../../../globalContext";


class AddComplexDiscount extends React.Component {
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

                    <div className="disc-comp-check-line-grid">

                        <div className="center-item">
                            <div className= "disc-col-grd-perm">
                                <div className="disc-text-props">
                                    <label>Discount Relation</label>
                                </div>

                                <div>
                                    <select className="disc-input-props">
                                        <option value="Xor">Xor</option>
                                        <option value="Or">Or</option>
                                        <option value="And">And</option>
                                    </select>
                                </div>
                            </div>
                        </div>

                        <div className="center-item">
                            <div className= "disc-col-grd-perm">
                                <div className="disc-text-props">
                                    <label>First Discount</label>
                                </div>

                                <div >
                                    <input
                                        type="text"
                                        className="disc-input-props"
                                    />
                                </div>
                            </div>
                        </div>

                        <div className="center-item">
                            <div className= "disc-col-grd-perm">
                                <div className="disc-text-props">
                                    <label>Second Discount</label>
                                </div>

                                <div >
                                    <input
                                        type="text"
                                        className="disc-input-props"
                                    />
                                </div>
                            </div>
                        </div>

                        <div className="center-item">
                            <div className= "disc-col-grd-perm">
                                <div className="disc-text-props">
                                    <label>Decision</label>
                                </div>

                                <div >
                                    <input
                                        type="checkbox"
                                        className="disc-input-props"
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
            <section className='disc-comp-modal-main'>

                <div className="lines-props">
                    <h2 className="comp-center-text">Add Complex Discount</h2>
                    {children}
                </div>


                <div className="comp-modal-buttons">
                    <button className="modal-buttons-props" onClick={handleClose} > Close </button>
                    <button className="modal-buttons-props" onClick={handleConfirm} > Add </button>
                </div>


            </section>
        </div>
    );
};

AddComplexDiscount.contextType = GlobalContext;
export default AddComplexDiscount
