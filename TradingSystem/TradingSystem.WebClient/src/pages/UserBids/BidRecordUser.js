import React from "react";
import {GlobalContext} from "../../globalContext";
import * as api from '../../api'
import NumberFormField from "../../formsUtil/NumberFormField";
import {alertRequestError_default} from "../../utils";
import ConditionalRender from "../../ConditionalRender";
import SimpleModal from "../../components/SimpleModal";
import FormFieldInfo from "../../formsUtil/formFieldInfo";
import BidEditContent from "../Stores/StoreBids/BidEditContent";

export default class BidRecordUser extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            inCart: false,
            quantity: new NumberFormField(),
            bidPrice: new NumberFormField(),
            action: new FormFieldInfo(),
            showDeclineModal: false,
            showApproveModal: false,
            showNegotiateModal: false,
        };
    }

    addToCart = async () => {
        await api.shoppingCart.addProduct({
            username: this.context.username,
            productId: this.props.bid.productId,
            quantity: this.state.quantity.getValue(),
        }).then(response => {
            this.setState({
                inCart: true,
                quantity: new NumberFormField(),
            });
        }, alertRequestError_default);
    }

    goToShoppingCart = () => {
        this.props.history.push('/ShoppingCart');
    }

    onQuantityChange = e => {
        if (!this.state.quantity.trySetValueFromEvent(e)) {
            return;
        }

        this.setState({
            ...this.state
        });
    }

    onPriceChange = e => {
        if (!this.state.bidPrice.trySetValueFromEvent(e)) {
            return;
        }

        this.setState({
            ...this.state
        });
    }

    onActionSelectionChange = e => {
        if (!this.state.action.trySetValueFromEvent(e)) {
            return;
        }

        this.setState({
            ...this.state
        });
    }

    applyAction = () => {
        switch (this.state.action.getValue()) {
            case '':
                break;

            case 'Approve':
                this.openApproveBidModal();
                break;

            case 'Negotiate':
                this.openNegotiateBidModal();
                break;

            case 'Decline':
                this.openDeclineBidModal();
                break;

            default:
                throw new Error('Invalid bid action');
        }
    }

    openApproveBidModal() {
        this.setState({
            showApproveModal: true,
        });
    }

    hideApproveBidModal = () => {
        this.setState({
            showApproveModal: false,
        });
    }

    approveBid = () => {
        // TODO: complete
        console.log('Approve');
    }

    openNegotiateBidModal() {
        this.setState({
            showNegotiateModal: true,
        });
    }

    hideNegotiateBidModal = () => {
        this.setState({
            showNegotiateModal: false,
        });
    }

    negotiateBid = () => {
        // TODO: complete
        console.log('negotiate', this.state.bidPrice.getValue());
    }

    openDeclineBidModal() {
        this.setState({
            showDeclineModal: true,
        });
    }

    hideDeclineBidModal = () => {
        this.setState({
            showDeclineModal: false,
        });
    }

    declineBid = () => {
        // TODO: complete
        console.log('Decline');
    }

    render() {
        let bid = this.props.bid;
        let isInNegotiation = (
            bid.status === api.data.bids.customerNegotiate ||
            bid.status === api.data.bids.ownerNegotiate
        );

        let declineText = bid.status === api.data.bids.customerNegotiate ? 'Cancel' : 'Decline';

        return (
            <div className = "simple-bids-li-div">
                <p className= "bidName">{<text style={{fontWeight: "bold"}}>Store: </text>} {bid.storeName} </p>
                <p className= "bidName"> {<text style={{fontWeight: "bold"}}>Product: </text>} {bid.productName}  </p>
                <p className= "bidName"> {<text style={{fontWeight: "bold"}}>Current Offer:</text>} {bid.price}  </p>
                <p className= "bidName">{<text style={{fontWeight: "bold"}}>Status:</text>} {
                    bid.status === api.data.bids.approved ? (<label style={{color: "green"}}>Approved</label>) :
                    bid.status === api.data.bids.ownerNegotiate ? (<label style={{color: "orange"}}>Negotiation (your turn)</label>) :
                    bid.status === api.data.bids.customerNegotiate ? (<label style={{color: "orange"}}>Negotiation (waiting on store)</label>) :
                    bid.status === api.data.bids.declined ? (<label style={{color: "red"}}>Declined</label>) :
                    null
                }
                </p>

                <ConditionalRender
                    condition={!this.state.inCart && isInNegotiation}
                    render={() => (
                        <p className= "bidName">
                            <text style={{fontWeight: "bold"}}>Action: </text>
                            <select onChange={this.onActionSelectionChange}>
                                <option value=""/>

                                {/*if Approve is chosen a pop-up should appear to warn about Approving the offer permanently */}
                                {bid.status === api.data.bids.ownerNegotiate ? (<option value="Approve">Approve</option>) : null}
                                {/*if Negotiate is chosen a pop-up should appear to update the current price */}
                                <option value="Negotiate">Negotiate</option>
                                {/*if Decline is chosen a pop-up should appear to warn about declining the offer permanently */}
                                <option value="Decline">{declineText}</option>
                            </select>
                        </p>
                    )}
                />

                {!this.state.inCart && isInNegotiation ?
                    (<button className="button primary" style={{margin: "2rem"}} onClick={this.applyAction}>Apply Action</button>) : null
                }
                {!this.state.inCart && bid.status === api.data.bids.approved ? (
                    <input
                        type="number"
                        placeholder="Quantity"
                        style={{width: "8rem", height: "4rem", marginLeft:"10rem", marginBottom:"2rem", textAlign:"center"}}
                        required
                        value={this.state.quantity.getInputValue()}
                        onChange={this.onQuantityChange}
                    />
                ) : null}
                {!this.state.inCart && bid.status === api.data.bids.approved ? (<button className="button primary" style={{margin: "2rem"}} onClick={this.addToCart}>Add To Cart</button>) : null}
                {this.state.inCart ? (<button className="button primary" style={{margin: "2rem"}} onClick={this.goToShoppingCart}>Go to shopping cart</button>) : null}

                <SimpleModal
                    title={`${declineText} bid`}
                    show={this.state.showDeclineModal}
                    width={'500px'}
                    height={'215px'}
                    btn1Text={'Close'}
                    btn1Handle={this.hideDeclineBidModal}
                    btn2Text={'Decline'}
                    btn2Handle={this.declineBid}
                    btn2Class={'danger'}
                >
                    <span style={{
                        marginLeft: '2rem',
                        fontSize: '2rem',
                    }}>Are you sure you would like to decline the bid?</span>
                </SimpleModal>

                <SimpleModal
                    title={'Approve bid'}
                    show={this.state.showApproveModal}
                    width={'500px'}
                    height={'215px'}
                    btn1Text={'Close'}
                    btn1Handle={this.hideApproveBidModal}
                    btn2Text={'Approve'}
                    btn2Handle={this.approveBid}
                >
                    <span style={{
                        marginLeft: '2rem',
                        fontSize: '2rem',
                    }}>Approve this bid?</span>
                </SimpleModal>

                <SimpleModal
                    title={'Negotiate bid'}
                    show={this.state.showNegotiateModal}
                    width={'500px'}
                    height={'300px'}
                    btn1Text={'Close'}
                    btn1Handle={this.hideNegotiateBidModal}
                    btn2Text={'Bid'}
                    btn2Handle={this.negotiateBid}>
                    <BidEditContent
                        value={this.state.bidPrice.getInputValue()}
                        onChange={this.onPriceChange}
                        lineGrid={false} />
                </SimpleModal>
            </div>
        );
    }
}

BidRecordUser.contextType = GlobalContext;
