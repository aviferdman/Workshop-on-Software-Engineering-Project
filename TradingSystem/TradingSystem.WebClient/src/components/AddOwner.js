import React from "react";
import './AddManager.css';

class AddOwner extends React.Component {
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
                            <label >UserName</label>
                        </div>

                        <div >
                            <input
                                type="text"
                                placeholder="UserName"
                                className="input-props"
                            />
                        </div>
                    </div>

                </Modal>

                <button className= "store-products-button-view" onClick={this.showModal}> Add Owner </button>
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
                    <h2 className="center-text">Add Owner</h2>
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


export default AddOwner
