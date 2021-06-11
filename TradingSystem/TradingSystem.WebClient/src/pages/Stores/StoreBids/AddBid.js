import React from "react";
import '../Discount/AddSimpleDiscount.css';
import { GlobalContext } from "../../../globalContext";
import * as GiIcons from "react-icons/gi";
import * as AiIcons from "react-icons/ai";


class AddBid extends React.Component {
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
                                    <label>Bid Price</label>
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

                    </div>

                </Modal>

                <button  style={{cursor: "pointer", width:"4rem",}} onClick={this.showModal}> <GiIcons.GiTakeMyMoney /> </button>
            </main>
        )
    }
}

const Modal = ({ handleClose, handleConfirm, show, children }) => {
    if (!show) {
        return null;
    }

    return (
        <div className='modal display-block'>
            <section className='disc-comp-modal-main'>

                <div className="lines-props">
                    <h2 className="bidName">Add Bid</h2>
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

AddBid.contextType = GlobalContext;
export default AddBid
