import React from "react";
import '../Discount/AddSimpleDiscount.css';
import { GlobalContext } from "../../../globalContext";
import * as GiIcons from "react-icons/gi";
import NumberFormField from "../../../formsUtil/NumberFormField";
import * as api from "../../../api";
import {alertRequestError_default} from "../../../utils";

class AddBid extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            show: false,
            price: new NumberFormField(),
        };
    }

    showModal = () => {
        this.setState({
            show: true,
            price: new NumberFormField(),
        });
    }

    hideModal = () => {
        this.setState({
            show: false,
            price: new NumberFormField(),
        });
    }

    onPriceChange = e => {
        if (!this.state.price.trySetValueFromEvent(e)) {
            return;
        }

        this.setState({
            ...this.state
        });
    }

    onConfirm = async e => {
        e.preventDefault();
        if (!this.state.price.validate()) {
            alert('Please fill the proposed price');
            return;
        }

        await api.stores.bids.createCustomerBid({
            username: this.context.username,
            storeId: this.props.storeId,
            productId: this.props.product.id,
            newPrice: this.state.price.getValue(),
        }).then(bidId => {
            this.hideModal();
        }, alertRequestError_default);
    }

    render() {
        return (
            <main className="items">
                <Modal show={this.state.show} handleClose={this.hideModal} handleConfirm={this.onConfirm} >

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
                                        required
                                        value={this.state.price.getInputValue()}
                                        onChange={this.onPriceChange}
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
